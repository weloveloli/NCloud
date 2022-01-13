// -----------------------------------------------------------------------
// <copyright file="RequestHandlerFactory.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server
{
    using System.Collections.Generic;
    using NWebDav.Server.Handlers;
    using NWebDav.Server.Http;

    /// <summary>
    /// Default implementation of the <see cref="IRequestHandlerFactory"/>
    /// interface to create WebDAV request handlers.
    /// </summary>
    public class RequestHandlerFactory : IRequestHandlerFactory
    {
        /// <summary>
        /// Defines the s_requestHandlers.
        /// </summary>
        private static readonly IDictionary<string, IRequestHandler> s_requestHandlers = new Dictionary<string, IRequestHandler>
        {
            { "COPY", new CopyHandler() },
            { "DELETE", new DeleteHandler() },
            { "GET", new GetAndHeadHandler() },
            { "HEAD", new GetAndHeadHandler() },
            { "LOCK", new LockHandler() },
            { "MKCOL", new MkcolHandler() },
            { "MOVE", new MoveHandler() },
            { "OPTIONS", new OptionsHandler() },
            { "PROPFIND", new PropFindHandler() },
            { "PROPPATCH", new PropPatchHandler() },
            { "PUT", new PutHandler() },
            { "UNLOCK", new UnlockHandler() }
        };

        /// <summary>
        /// Obtain the <seealso cref="IRequestHandler">request handler</seealso>
        /// that can process the specified request.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="IRequestHandler"/>.</returns>
        public IRequestHandler GetRequestHandler(IHttpContext httpContext)
        {
            // Obtain the dispatcher
            if (!s_requestHandlers.TryGetValue(httpContext.Request.HttpMethod, out var requestHandler))
                return null;

            // Create an instance of the request handler
            return requestHandler;
        }

        /// <summary>
        /// Gets the AllowedMethods
        /// Gets a list of supported HTTP methods..
        /// </summary>
        public static IEnumerable<string> AllowedMethods => s_requestHandlers.Keys;

        /// <summary>
        /// Gets the ReadOnlyMethods.
        /// </summary>
        public static IEnumerable<string> ReadOnlyMethods => new string[] { "GET", "HEAD", "OPTIONS" };
    }
}
