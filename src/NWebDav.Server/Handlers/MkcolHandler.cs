// -----------------------------------------------------------------------
// <copyright file="MkcolHandler.cs" company="Weloveloli">
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
    /// Implementation of the MKCOL method.
    /// </summary>
    public class MkcolHandler : IRequestHandler
    {
        /// <summary>
        /// Handle a MKCOL request.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="store">The store<see cref="IStore"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> HandleRequestAsync(IHttpContext httpContext, IStore store)
        {
            // Obtain request and response
            var request = httpContext.Request;
            var response = httpContext.Response;

            // The collection must always be created inside another collection
            var splitUri = RequestHelper.SplitUri(request.Url);

            // Obtain the parent entry
            var collection = await store.GetCollectionAsync(splitUri.CollectionUri, httpContext).ConfigureAwait(false);
            if (collection == null)
            {
                // Source not found
                response.SetStatus(DavStatusCode.Conflict);
                return true;
            }

            // Create the collection
            var result = await collection.CreateCollectionAsync(splitUri.Name, false, httpContext).ConfigureAwait(false);

            // Finished
            response.SetStatus(result.Result);
            return true;
        }
    }
}
