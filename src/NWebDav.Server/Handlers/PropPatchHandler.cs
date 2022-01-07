// -----------------------------------------------------------------------
// <copyright file="PropPatchHandler.cs" company="Weloveloli">
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
    using NWebDav.Server.Stores;

    /// <summary>
    /// Implementation of the PROPPATCH method.
    /// </summary>
    public class PropPatchHandler : IRequestHandler
    {
        /// <summary>
        /// Defines the <see cref="PropSetCollection" />.
        /// </summary>
        private class PropSetCollection : List<PropSetCollection.PropSet>
        {
            /// <summary>
            /// Defines the <see cref="PropSet" />.
            /// </summary>
            public class PropSet
            {
                /// <summary>
                /// Gets the Name.
                /// </summary>
                public XName Name { get; }

                /// <summary>
                /// Gets the Value.
                /// </summary>
                public object Value { get; }

                /// <summary>
                /// Gets or sets the Result.
                /// </summary>
                public DavStatusCode Result { get; set; }

                /// <summary>
                /// Initializes a new instance of the <see cref="PropSet"/> class.
                /// </summary>
                /// <param name="name">The name<see cref="XName"/>.</param>
                /// <param name="value">The value<see cref="object"/>.</param>
                public PropSet(XName name, object value)
                {
                    Name = name;
                    Value = value;
                }

                /// <summary>
                /// The GetXmlResponse.
                /// </summary>
                /// <returns>The <see cref="XElement"/>.</returns>
                public XElement GetXmlResponse()
                {
                    var statusText = $"HTTP/1.1 {(int)Result} {Result.GetStatusDescription()}";
                    return new XElement(WebDavNamespaces.DavNs + "propstat",
                        new XElement(WebDavNamespaces.DavNs + "prop", new XElement(Name)),
                        new XElement(WebDavNamespaces.DavNs + "status", statusText));
                }
            }

            /// <summary>
            /// Defines the _propertySetters.
            /// </summary>
            private readonly IList<PropSet> _propertySetters = new List<PropSet>();

            /// <summary>
            /// Initializes a new instance of the <see cref="PropSetCollection"/> class.
            /// </summary>
            /// <param name="xPropertyUpdate">The xPropertyUpdate<see cref="XElement"/>.</param>
            public PropSetCollection(XElement xPropertyUpdate)
            {
                // The document should contain a 'propertyupdate' root element
                if (xPropertyUpdate == null || xPropertyUpdate.Name != WebDavNamespaces.DavNs + "propertyupdate")
                    throw new Exception("Invalid root element (expected 'propertyupdate')");

                // Check all descendants
                foreach (var xElement in xPropertyUpdate.Elements())
                {
                    // The descendant should be a 'set' or 'remove' entry
                    if (xElement.Name != WebDavNamespaces.DavNs + "set" && xElement.Name != WebDavNamespaces.DavNs + "remove")
                        throw new Exception("Expected 'set' or 'remove' entry");

                    // Obtain the properties
                    foreach (var xProperty in xElement.Descendants(WebDavNamespaces.DavNs + "prop"))
                    {
                        // Determine the actual property element
                        var xActualProperty = xProperty.Elements().FirstOrDefault();
                        if (xActualProperty != null)
                        {
                            // Determine the new property value
                            object newValue;
                            if (xElement.Name == WebDavNamespaces.DavNs + "set")
                            {
                                // If the descendant is XML, then use the XElement, otherwise use the string
                                newValue = xActualProperty.HasElements ? (object)xActualProperty.Elements().FirstOrDefault() : xActualProperty.Value;
                            }
                            else
                            {
                                newValue = null;
                            }

                            // Add the property
                            _propertySetters.Add(new PropSet(xActualProperty.Name, newValue));
                        }
                    }
                }
            }

            /// <summary>
            /// The GetXmlMultiStatus.
            /// </summary>
            /// <param name="uri">The uri<see cref="Uri"/>.</param>
            /// <returns>The <see cref="XElement"/>.</returns>
            public XElement GetXmlMultiStatus(Uri uri)
            {
                var xResponse = new XElement(WebDavNamespaces.DavNs + "response", new XElement(WebDavNamespaces.DavNs + "href", UriHelper.ToEncodedString(uri)));
                var xMultiStatus = new XElement(WebDavNamespaces.DavNs + "multistatus", xResponse);
                foreach (var result in _propertySetters.Where(ps => ps.Result != DavStatusCode.Ok))
                    xResponse.Add(result.GetXmlResponse());
                return xMultiStatus;
            }
        }

        /// <summary>
        /// Handle a PROPPATCH request.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="store">The store<see cref="IStore"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> HandleRequestAsync(IHttpContext httpContext, IStore store)
        {
            // Obtain request and response
            var request = httpContext.Request;
            var response = httpContext.Response;

            // Obtain item
            var item = await store.GetItemAsync(request.Url, httpContext).ConfigureAwait(false);
            if (item == null)
            {
                response.SetStatus(DavStatusCode.NotFound);
                return true;
            }

            // Read the property set/remove items from the request
            PropSetCollection propSetCollection;
            try
            {
                // Create an XML document from the stream
                var xDoc = await request.LoadXmlDocumentAsync().ConfigureAwait(false);

                // Create an XML document from the stream
                propSetCollection = new PropSetCollection(xDoc.Root);
            }
            catch (Exception)
            {
                response.SetStatus(DavStatusCode.BadRequest);
                return true;
            }

            // Scan each property
            foreach (var propSet in propSetCollection)
            {
                // Set the property
                DavStatusCode result;
                try
                {
                    result = await item.PropertyManager.SetPropertyAsync(httpContext, item, propSet.Name, propSet.Value).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    result = DavStatusCode.Forbidden;
                }

                propSet.Result = result;
            }

            // Obtain the status document
            var xDocument = new XDocument(propSetCollection.GetXmlMultiStatus(request.Url));

            // Stream the document
            await response.SendResponseAsync(DavStatusCode.MultiStatus, xDocument).ConfigureAwait(false);
            return true;
        }
    }
}
