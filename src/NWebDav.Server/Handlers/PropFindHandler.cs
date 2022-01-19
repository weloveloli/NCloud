// -----------------------------------------------------------------------
// <copyright file="PropFindHandler.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using NWebDav.Server.Helpers;
    using NWebDav.Server.Http;
    using NWebDav.Server.Logging;
    using NWebDav.Server.Props;
    using NWebDav.Server.Stores;

    /// <summary>
    /// Implementation of the PROPFIND method.
    /// </summary>
    public class PropFindHandler : IRequestHandler
    {
        /// <summary>
        /// Defines the <see cref="PropertyEntry" />.
        /// </summary>
        private struct PropertyEntry
        {
            /// <summary>
            /// Gets the Uri.
            /// </summary>
            public Uri Uri { get; }

            /// <summary>
            /// Gets the Entry.
            /// </summary>
            public IStoreItem Entry { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref=""/> class.
            /// </summary>
            /// <param name="uri">The uri<see cref="Uri"/>.</param>
            /// <param name="entry">The entry<see cref="IStoreItem"/>.</param>
            public PropertyEntry(Uri uri, IStoreItem entry)
            {
                Uri = uri;
                Entry = entry;
            }
        }

        /// <summary>
        /// Defines the PropertyMode.
        /// </summary>
        [Flags]
        private enum PropertyMode
        {
            /// <summary>
            /// Defines the None.
            /// </summary>
            None = 0,

            /// <summary>
            /// Defines the PropertyNames.
            /// </summary>
            PropertyNames = 1,

            /// <summary>
            /// Defines the AllProperties.
            /// </summary>
            AllProperties = 2,

            /// <summary>
            /// Defines the SelectedProperties.
            /// </summary>
            SelectedProperties = 4
        }

        /// <summary>
        /// Defines the s_log.
        /// </summary>
        private static readonly ILogger s_log = LoggerFactory.CreateLogger(typeof(PropFindHandler));

        /// <summary>
        /// Handle a PROPFIND request.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="store">The store<see cref="IStore"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> HandleRequestAsync(IHttpContext httpContext, IStore store)
        {
            // Obtain request and response
            var request = httpContext.Request;
            var response = httpContext.Response;

            // Determine the list of properties that need to be obtained
            var propertyList = new List<XName>();
            var propertyMode = await GetRequestedPropertiesAsync(request, propertyList).ConfigureAwait(false);

            // Generate the list of items from which we need to obtain the properties
            var entries = new List<PropertyEntry>();

            // Obtain entry
            var topEntry = await store.GetItemAsync(request.Url, httpContext).ConfigureAwait(false);
            if (topEntry == null)
            {
                response.SetStatus(DavStatusCode.NotFound);
                return true;
            }

            // Check if the entry is a collection
            if (topEntry is IStoreCollection topCollection)
            {
                // Determine depth
                var depth = request.GetDepth();

                // Check if the collection supports Infinite depth for properties
                if (depth > 1)
                {
                    switch (topCollection.InfiniteDepthMode)
                    {
                        case InfiniteDepthMode.Rejected:
                            response.SetStatus(DavStatusCode.Forbidden, "Not allowed to obtain properties with infinite depth.");
                            return true;
                        case InfiniteDepthMode.Assume0:
                            depth = 0;
                            break;
                        case InfiniteDepthMode.Assume1:
                            depth = 1;
                            break;
                    }
                }

                // Add all the entries
                await AddEntriesAsync(topCollection, depth, httpContext, request.Url, entries).ConfigureAwait(false);
            }
            else
            {
                // It should be an item, so just use this item
                entries.Add(new PropertyEntry(request.Url, topEntry));
            }

            // Obtain the status document
            var xMultiStatus = new XElement(WebDavNamespaces.DavNs + "multistatus");
            var xDocument = new XDocument(xMultiStatus);

            // Add all the properties
            foreach (var entry in entries)
            {
                // Create the property
                var xResponse = new XElement(WebDavNamespaces.DavNs + "response",
                    new XElement(WebDavNamespaces.DavNs + "href", UriHelper.ToEncodedString(entry.Uri)));

                // Create tags for property values
                var xPropStatValues = new XElement(WebDavNamespaces.DavNs + "propstat");

                // Check if the entry supports properties
                var propertyManager = entry.Entry.PropertyManager;
                if (propertyManager != null)
                {
                    // Handle based on the property mode
                    if (propertyMode == PropertyMode.PropertyNames)
                    {
                        // Add all properties
                        foreach (var property in propertyManager.Properties)
                            xPropStatValues.Add(new XElement(property.Name));

                        // Add the values
                        xResponse.Add(xPropStatValues);
                    }
                    else
                    {
                        var addedProperties = new List<XName>();
                        if ((propertyMode & PropertyMode.AllProperties) != 0)
                        {
                            foreach (var propertyName in propertyManager.Properties.Where(p => !p.IsExpensive).Select(p => p.Name))
                            {
                                await AddPropertyAsync(httpContext, xResponse, xPropStatValues, propertyManager, entry.Entry, propertyName, addedProperties).ConfigureAwait(false);
                            }
                        }

                        if ((propertyMode & PropertyMode.SelectedProperties) != 0)
                        {
                            foreach (var propertyName in propertyList)
                            {
                                await AddPropertyAsync(httpContext, xResponse, xPropStatValues, propertyManager, entry.Entry, propertyName, addedProperties).ConfigureAwait(false);
                            }
                        }

                        // Add the values (if any)
                        if (xPropStatValues.HasElements)
                        {
                            xResponse.Add(xPropStatValues);
                        }
                    }
                }

                // Add the status
                xPropStatValues.Add(new XElement(WebDavNamespaces.DavNs + "status", "HTTP/1.1 200 OK"));

                // Add the property
                xMultiStatus.Add(xResponse);
            }

            // Stream the document
            await response.SendResponseAsync(DavStatusCode.MultiStatus, xDocument).ConfigureAwait(false);

            // Finished writing
            return true;
        }

        /// <summary>
        /// The AddPropertyAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="xResponse">The xResponse<see cref="XElement"/>.</param>
        /// <param name="xPropStatValues">The xPropStatValues<see cref="XElement"/>.</param>
        /// <param name="propertyManager">The propertyManager<see cref="IPropertyManager"/>.</param>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <param name="propertyName">The propertyName<see cref="XName"/>.</param>
        /// <param name="addedProperties">The addedProperties<see cref="IList{XName}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task AddPropertyAsync(IHttpContext httpContext, XElement xResponse, XElement xPropStatValues, IPropertyManager propertyManager, IStoreItem item, XName propertyName, IList<XName> addedProperties)
        {
            if (!addedProperties.Contains(propertyName))
            {
                addedProperties.Add(propertyName);
                try
                {
                    // Check if the property is supported
                    if (propertyManager.Properties.Any(p => p.Name == propertyName))
                    {
                        var value = await propertyManager.GetPropertyAsync(httpContext, item, propertyName).ConfigureAwait(false);
                        if (value is IEnumerable<XElement>)
                        {
                            value = ((IEnumerable<XElement>)value).Cast<object>().ToArray();
                        }

                        // Make sure we use the same 'prop' tag to add all properties
                        var xProp = xPropStatValues.Element(WebDavNamespaces.DavNs + "prop");
                        if (xProp == null)
                        {
                            xProp = new XElement(WebDavNamespaces.DavNs + "prop");
                            xPropStatValues.Add(xProp);
                        }

                        xProp.Add(new XElement(propertyName, value));
                    }
                    else
                    {
                        s_log.Log(LogLevel.Warning, () => $"Property {propertyName} is not supported on item {item.Name}.");
                        xResponse.Add(new XElement(WebDavNamespaces.DavNs + "propstat",
                            new XElement(WebDavNamespaces.DavNs + "prop", new XElement(propertyName, null)),
                            new XElement(WebDavNamespaces.DavNs + "status", "HTTP/1.1 404 Not Found"),
                            new XElement(WebDavNamespaces.DavNs + "responsedescription", $"Property {propertyName} is not supported.")));
                    }
                }
                catch (Exception exc)
                {
                    s_log.Log(LogLevel.Error, () => $"Property {propertyName} on item {item.Name} raised an exception.", exc);
                    xResponse.Add(new XElement(WebDavNamespaces.DavNs + "propstat",
                        new XElement(WebDavNamespaces.DavNs + "prop", new XElement(propertyName, null)),
                        new XElement(WebDavNamespaces.DavNs + "status", "HTTP/1.1 500 Internal server error"),
                        new XElement(WebDavNamespaces.DavNs + "responsedescription", $"Property {propertyName} on item {item.Name} raised an exception.")));
                }
            }
        }

        /// <summary>
        /// The GetRequestedPropertiesAsync.
        /// </summary>
        /// <param name="request">The request<see cref="IHttpRequest"/>.</param>
        /// <param name="properties">The properties<see cref="ICollection{XName}"/>.</param>
        /// <returns>The <see cref="Task{PropertyMode}"/>.</returns>
        private static async Task<PropertyMode> GetRequestedPropertiesAsync(IHttpRequest request, ICollection<XName> properties)
        {
            // Create an XML document from the stream
            var xDocument = await request.LoadXmlDocumentAsync().ConfigureAwait(false);
            if (xDocument == null || xDocument?.Root == null || xDocument.Root.Name != WebDavNamespaces.DavNs + "propfind")
            {
                return PropertyMode.AllProperties;
            }

            // Obtain the propfind node
            var xPropFind = xDocument.Root;

            // If there is no child-node, then return all properties
            var xProps = xPropFind.Elements();
            if (!xProps.Any())
            {
                return PropertyMode.AllProperties;
            }

            // Add all entries to the list
            var propertyMode = PropertyMode.None;
            foreach (var xProp in xPropFind.Elements())
            {
                // Check if we should fetch all property names
                if (xProp.Name == WebDavNamespaces.DavNs + "propname")
                {
                    propertyMode = PropertyMode.PropertyNames;
                }
                else if (xProp.Name == WebDavNamespaces.DavNs + "allprop")
                {
                    propertyMode = PropertyMode.AllProperties;
                }
                else if (xProp.Name == WebDavNamespaces.DavNs + "include")
                {
                    // Include properties
                    propertyMode = PropertyMode.AllProperties | PropertyMode.SelectedProperties;

                    // Include all specified properties
                    foreach (var xSubProp in xProp.Elements())
                    {
                        properties.Add(xSubProp.Name);
                    }
                }
                else
                {
                    propertyMode = PropertyMode.SelectedProperties;

                    // Include all specified properties
                    foreach (var xSubProp in xProp.Elements())
                    {
                        properties.Add(xSubProp.Name);
                    }
                }
            }

            return propertyMode;
        }

        /// <summary>
        /// The AddEntriesAsync.
        /// </summary>
        /// <param name="collection">The collection<see cref="IStoreCollection"/>.</param>
        /// <param name="depth">The depth<see cref="int"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="entries">The entries<see cref="IList{PropertyEntry}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task AddEntriesAsync(IStoreCollection collection, int depth, IHttpContext httpContext, Uri uri, IList<PropertyEntry> entries)
        {
            // Add the collection to the list
            entries.Add(new PropertyEntry(uri, collection));

            // If we have enough depth, then add the children
            if (depth > 0)
            {
                if(depth == 1)
                {

                }
                // Add all child collections
                foreach (var childEntry in await collection.GetItemsAsync(httpContext).ConfigureAwait(false))
                {
                    var subUri = UriHelper.Combine(uri, childEntry.Name);
                    if (childEntry is IStoreCollection subCollection)
                    {
                        await AddEntriesAsync(subCollection, depth - 1, httpContext, subUri, entries).ConfigureAwait(false);
                    }
                    else
                    {
                        entries.Add(new PropertyEntry(subUri, childEntry));
                    }
                }
            }
        }
    }
}
