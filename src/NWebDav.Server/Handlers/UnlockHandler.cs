// -----------------------------------------------------------------------
// <copyright file="UnlockHandler.cs" company="Weloveloli">
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
    /// Implementation of the UNLOCK method.
    /// </summary>
    public class UnlockHandler : IRequestHandler
    {
        /// <summary>
        /// Handle a UNLOCK request.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="store">The store<see cref="IStore"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> HandleRequestAsync(IHttpContext httpContext, IStore store)
        {
            // Obtain request and response
            var request = httpContext.Request;
            var response = httpContext.Response;

            // Obtain the lock-token
            var lockToken = request.GetLockToken();

            // Obtain the WebDAV item
            var item = await store.GetItemAsync(request.Url, httpContext).ConfigureAwait(false);
            if (item == null)
            {
                // Set status to not found
                response.SetStatus(DavStatusCode.PreconditionFailed);
                return true;
            }

            // Check if we have a lock manager
            var lockingManager = item.LockingManager;
            if (lockingManager == null)
            {
                // Set status to not found
                response.SetStatus(DavStatusCode.PreconditionFailed);
                return true;
            }

            // Perform the lock
            var result = lockingManager.Unlock(item, lockToken);

            // Send response
            response.SetStatus(result);
            return true;
        }
    }
}
