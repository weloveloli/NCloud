// -----------------------------------------------------------------------
// <copyright file="CopyHandler.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Handlers
{
    using System;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using NWebDav.Server.Helpers;
    using NWebDav.Server.Http;
    using NWebDav.Server.Stores;

    /// <summary>
    /// Implementation of the COPY method.
    /// </summary>
    public class CopyHandler : IRequestHandler
    {
        /// <summary>
        /// Handle a COPY request.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="store">The store<see cref="IStore"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> HandleRequestAsync(IHttpContext httpContext, IStore store)
        {
            // Obtain request and response
            var request = httpContext.Request;
            var response = httpContext.Response;

            // Obtain the destination
            var destinationUri = request.GetDestinationUri();
            if (destinationUri == null)
            {
                // Bad request
                response.SetStatus(DavStatusCode.BadRequest, "Destination header is missing.");
                return true;
            }

            // Make sure the source and destination are different
            if (request.Url.AbsoluteUri.Equals(destinationUri.AbsoluteUri, StringComparison.CurrentCultureIgnoreCase))
            {
                // Forbidden
                response.SetStatus(DavStatusCode.Forbidden, "Source and destination cannot be the same.");
                return true;
            }

            // Check if the Overwrite header is set
            var overwrite = request.GetOverwrite();

            // Split the destination Uri
            var destination = RequestHelper.SplitUri(destinationUri);

            // Obtain the destination collection
            var destinationCollection = await store.GetCollectionAsync(destination.CollectionUri, httpContext).ConfigureAwait(false);
            if (destinationCollection == null)
            {
                // Source not found
                response.SetStatus(DavStatusCode.Conflict, "Destination cannot be found or is not a collection.");
                return true;
            }

            // Obtain the source item
            var sourceItem = await store.GetItemAsync(request.Url, httpContext).ConfigureAwait(false);
            if (sourceItem == null)
            {
                // Source not found
                response.SetStatus(DavStatusCode.NotFound, "Source cannot be found.");
                return true;
            }

            // Determine depth
            var depth = request.GetDepth();

            // Keep track of all errors
            var errors = new UriResultCollection();

            // Copy collection
            await CopyAsync(sourceItem, destinationCollection, destination.Name, overwrite, depth, httpContext, destination.CollectionUri, errors).ConfigureAwait(false);

            // Check if there are any errors
            if (errors.HasItems)
            {
                // Obtain the status document
                var xDocument = new XDocument(errors.GetXmlMultiStatus());

                // Stream the document
                await response.SendResponseAsync(DavStatusCode.MultiStatus, xDocument).ConfigureAwait(false);
            }
            else
            {
                // Set the response
                response.SetStatus(DavStatusCode.Ok);
            }

            return true;
        }

        /// <summary>
        /// The CopyAsync.
        /// </summary>
        /// <param name="source">The source<see cref="IStoreItem"/>.</param>
        /// <param name="destinationCollection">The destinationCollection<see cref="IStoreCollection"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="depth">The depth<see cref="int"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="baseUri">The baseUri<see cref="Uri"/>.</param>
        /// <param name="errors">The errors<see cref="UriResultCollection"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task CopyAsync(IStoreItem source, IStoreCollection destinationCollection, string name, bool overwrite, int depth, IHttpContext httpContext, Uri baseUri, UriResultCollection errors)
        {
            // Determine the new base Uri
            var newBaseUri = UriHelper.Combine(baseUri, name);

            // Copy the item
            var copyResult = await source.CopyAsync(destinationCollection, name, overwrite, httpContext).ConfigureAwait(false);
            if (copyResult.Result != DavStatusCode.Created && copyResult.Result != DavStatusCode.NoContent)
            {
                errors.AddResult(newBaseUri, copyResult.Result);
                return;
            }

            // Check if the source is a collection and we are requested to copy recursively
            var sourceCollection = source as IStoreCollection;
            if (sourceCollection != null && depth > 0)
            {
                // The result should also contain a collection
                var newCollection = (IStoreCollection)copyResult.Item;

                // Copy all childs of the source collection
                foreach (var entry in await sourceCollection.GetItemsAsync(httpContext).ConfigureAwait(false))
                    await CopyAsync(entry, newCollection, entry.Name, overwrite, depth - 1, httpContext, newBaseUri, errors).ConfigureAwait(false);
            }
        }
    }
}
