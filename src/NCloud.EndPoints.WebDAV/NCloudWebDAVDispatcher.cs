// -----------------------------------------------------------------------
// <copyright file="NCloudWebDAVDispatcher.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.WebDAV
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NWebDav.Server;
    using NWebDav.Server.Http;
    using NWebDav.Server.Stores;

    /// <summary>
    /// Defines the <see cref="NCloudWebDAVDispatcher" />.
    /// </summary>
    public class NCloudWebDAVDispatcher : IWebDavDispatcher
    {
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<NCloudWebDAVDispatcher> logger;

        /// <summary>
        /// Defines the dispatcher.
        /// </summary>
        private WebDavDispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudWebDAVDispatcher"/> class.
        /// </summary>
        /// <param name="store">The store<see cref="IStore"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{NCloudWebDAVDispatcher}"/>.</param>
        public NCloudWebDAVDispatcher(IStore store, ILogger<NCloudWebDAVDispatcher> logger)
        {
            var requestHandlerFactory = new RequestHandlerFactory();
            this.dispatcher = new WebDavDispatcher(store, requestHandlerFactory);
            this.logger = logger;
        }

        /// <summary>
        /// The DispatchRequestAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task DispatchRequestAsync(IHttpContext httpContext)
        {
            logger.LogDebug("DispatchRequestAsync url:{url}, method:{method}", httpContext.Request.Url, httpContext.Request.HttpMethod);
            return dispatcher.DispatchRequestAsync(httpContext);
        }
    }
}
