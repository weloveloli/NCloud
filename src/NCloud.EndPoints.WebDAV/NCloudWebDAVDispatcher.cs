// -----------------------------------------------------------------------
// <copyright file="NCloudWebDAVDispatcher.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.WebDAV
{
    using System.Threading.Tasks;
    using NWebDav.Server;
    using NWebDav.Server.Http;
    using NWebDav.Server.Stores;

    /// <summary>
    /// Defines the <see cref="NCloudWebDAVDispatcher" />.
    /// </summary>
    public class NCloudWebDAVDispatcher : IWebDavDispatcher
    {
        /// <summary>
        /// Defines the dispatcher.
        /// </summary>
        private WebDavDispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudWebDAVDispatcher"/> class.
        /// </summary>
        /// <param name="store">The store<see cref="IStore"/>.</param>
        public NCloudWebDAVDispatcher(IStore store)
        {
            var requestHandlerFactory = new RequestHandlerFactory();
            this.dispatcher = new WebDavDispatcher(store, requestHandlerFactory);
        }

        /// <summary>
        /// The DispatchRequestAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task DispatchRequestAsync(IHttpContext httpContext)
        {
            return dispatcher.DispatchRequestAsync(httpContext);
        }
    }
}
