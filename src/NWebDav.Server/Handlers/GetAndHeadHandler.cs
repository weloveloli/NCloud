// -----------------------------------------------------------------------
// <copyright file="GetAndHeadHandler.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Handlers
{
    using System.Threading.Tasks;
    using NWebDav.Server.Helpers;
    using NWebDav.Server.Http;
    using NWebDav.Server.Logging;
    using NWebDav.Server.Stores;

    /// <summary>
    /// Implementation of the GET and HEAD method.
    /// </summary>
    public class GetAndHeadHandler : IRequestHandler
    {
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAndHeadHandler"/> class.
        /// </summary>
        public GetAndHeadHandler()
        {
            this.logger = LoggerFactory.CreateLogger(typeof(GetAndHeadHandler));
        }

        /// <summary>
        /// Handle a GET or HEAD request.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="store">The store<see cref="IStore"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> HandleRequestAsync(IHttpContext httpContext, IStore store)
        {
            // Obtain request and response
            var request = httpContext.Request;
            var response = httpContext.Response;
            // Obtain the WebDAV collection
            var entry = await store.GetItemAsync(request.Url, httpContext).ConfigureAwait(false);
            if (entry == null)
            {
                // Set status to not found
                response.SetStatus(DavStatusCode.NotFound);
                return true;
            }
            return await entry.ServeGetRequest(httpContext);
        }

    }
}
