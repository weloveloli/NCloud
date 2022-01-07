// -----------------------------------------------------------------------
// <copyright file="MoveHandler.cs" company="Weloveloli">
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
    /// Implementation of the MOVE method.
    /// </summary>
    public class MoveHandler : IRequestHandler
    {
        /// <summary>
        /// Handle a MOVE request.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="store">The store<see cref="IStore"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> HandleRequestAsync(IHttpContext httpContext, IStore store)
        {
            // Obtain request and response
            var request = httpContext.Request;
            var response = httpContext.Response;

            // We should always move the item from a parent container
            var splitSourceUri = RequestHelper.SplitUri(request.Url);

            // Obtain source collection
            var sourceCollection = await store.GetCollectionAsync(splitSourceUri.CollectionUri, httpContext).ConfigureAwait(false);
            if (sourceCollection == null)
            {
                // Source not found
                response.SetStatus(DavStatusCode.NotFound);
                return true;
            }

            // Obtain the item to move
            var moveItem = await sourceCollection.GetItemAsync(splitSourceUri.Name, httpContext).ConfigureAwait(false);
            if (moveItem == null)
            {
                // Source not found
                response.SetStatus(DavStatusCode.NotFound);
                return true;
            }

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

            // We should always move the item to a parent
            var splitDestinationUri = RequestHelper.SplitUri(destinationUri);

            // Obtain destination collection
            var destinationCollection = await store.GetCollectionAsync(splitDestinationUri.CollectionUri, httpContext).ConfigureAwait(false);
            if (destinationCollection == null)
            {
                // Source not found
                response.SetStatus(DavStatusCode.NotFound);
                return true;
            }

            // Check if the Overwrite header is set
            var overwrite = request.GetOverwrite();
            if (!overwrite)
            {
                // If overwrite is false and destination exist ==> Precondition Failed
                var destItem = await destinationCollection.GetItemAsync(splitDestinationUri.Name, httpContext).ConfigureAwait(false);
                if (destItem != null)
                {
                    // Cannot overwrite destination item
                    response.SetStatus(DavStatusCode.PreconditionFailed, "Cannot overwrite destination item.");
                    return true;
                }
            }

            // Keep track of all errors
            var errors = new UriResultCollection();

            // Move collection
            await MoveAsync(sourceCollection, moveItem, destinationCollection, splitDestinationUri.Name, overwrite, httpContext, splitDestinationUri.CollectionUri, errors).ConfigureAwait(false);

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
        /// The MoveAsync.
        /// </summary>
        /// <param name="sourceCollection">The sourceCollection<see cref="IStoreCollection"/>.</param>
        /// <param name="moveItem">The moveItem<see cref="IStoreItem"/>.</param>
        /// <param name="destinationCollection">The destinationCollection<see cref="IStoreCollection"/>.</param>
        /// <param name="destinationName">The destinationName<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="baseUri">The baseUri<see cref="Uri"/>.</param>
        /// <param name="errors">The errors<see cref="UriResultCollection"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task MoveAsync(IStoreCollection sourceCollection, IStoreItem moveItem, IStoreCollection destinationCollection, string destinationName, bool overwrite, IHttpContext httpContext, Uri baseUri, UriResultCollection errors)
        {
            // Determine the new base URI
            var subBaseUri = UriHelper.Combine(baseUri, destinationName);

            // Obtain the actual item
            if (moveItem is IStoreCollection moveCollection && !moveCollection.SupportsFastMove(destinationCollection, destinationName, overwrite, httpContext))
            {
                // Create a new collection
                var newCollectionResult = await destinationCollection.CreateCollectionAsync(destinationName, overwrite, httpContext).ConfigureAwait(false);
                if (newCollectionResult.Result != DavStatusCode.Created && newCollectionResult.Result != DavStatusCode.NoContent)
                {
                    errors.AddResult(subBaseUri, newCollectionResult.Result);
                    return;
                }

                // Move all sub items
                foreach (var entry in await moveCollection.GetItemsAsync(httpContext).ConfigureAwait(false))
                    await MoveAsync(moveCollection, entry, newCollectionResult.Collection, entry.Name, overwrite, httpContext, subBaseUri, errors).ConfigureAwait(false);

                // Delete the source collection
                var deleteResult = await sourceCollection.DeleteItemAsync(moveItem.Name, httpContext).ConfigureAwait(false);
                if (deleteResult != DavStatusCode.Ok)
                    errors.AddResult(subBaseUri, newCollectionResult.Result);
            }
            else
            {
                // Items should be moved directly
                var result = await sourceCollection.MoveItemAsync(moveItem.Name, destinationCollection, destinationName, overwrite, httpContext).ConfigureAwait(false);
                if (result.Result != DavStatusCode.Created && result.Result != DavStatusCode.NoContent)
                    errors.AddResult(subBaseUri, result.Result);
            }
        }
    }
}
