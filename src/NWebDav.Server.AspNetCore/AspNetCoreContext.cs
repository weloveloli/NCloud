// -----------------------------------------------------------------------
// <copyright file="AspNetCoreContext.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.AspNetCore
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    using NWebDav.Server.Http;
    public partial class AspNetCoreContext : IHttpContext
    {
        public AspNetCoreContext(HttpContext httpContext)
        {
            // Make sure a valid HTTP context is specified
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            // Save request, response and session
            Request = new AspNetCoreRequest(httpContext.Request);
            Response = new AspNetCoreResponse(httpContext.Response);
            Session = new AspNetCoreSession(httpContext.User);
            HttpContext = httpContext;
        }

        public IHttpRequest Request { get; }
        public IHttpResponse Response { get; }
        public IHttpSession Session { get; }
        public HttpContext HttpContext { get; }

        public Task CloseAsync()
        {
            // Context is closed automatically
            return Task.FromResult(true);
        }
    }
}