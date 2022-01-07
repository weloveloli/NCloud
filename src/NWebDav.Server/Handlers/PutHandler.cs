// -----------------------------------------------------------------------
// <copyright file="PutHandler.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Handlers
{
    using System.Threading.Tasks;
    using NWebDav.Server.Helpers;
    using NWebDav.Server.Http;
    using NWebDav.Server.Stores;

    /// <summary>
    /// Implementation of the PUT method.
    /// </summary>
    public class PutHandler : IRequestHandler
    {
        /// <summary>
        /// Handle a PUT request.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="store">The store<see cref="IStore"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> HandleRequestAsync(IHttpContext httpContext, IStore store)
        {
            // Obtain request and response
            var request = httpContext.Request;
            var response = httpContext.Response;

            // It's not a collection, so we'll try again by fetching the item in the parent collection
            var splitUri = RequestHelper.SplitUri(request.Url);

            // Obtain collection
            var collection = await store.GetCollectionAsync(splitUri.CollectionUri, httpContext).ConfigureAwait(false);
            if (collection == null)
            {
                // Source not found
                response.SetStatus(DavStatusCode.Conflict);
                return true;
            }

            // Obtain the item
            var result = await collection.CreateItemAsync(splitUri.Name, true, httpContext).ConfigureAwait(false);
            var status = result.Result;
            if (status == DavStatusCode.Created || status == DavStatusCode.NoContent)
            {
                // Upload the information to the item
                var uploadStatus = await result.Item.UploadFromStreamAsync(httpContext, request.Stream).ConfigureAwait(false);
                if (uploadStatus != DavStatusCode.Ok)
                    status = uploadStatus;
            }

            // Finished writing
            response.SetStatus(status);
            return true;
        }
    }
}
