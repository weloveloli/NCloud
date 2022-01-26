// -----------------------------------------------------------------------
// <copyright file="NCloudWebDAVDispatcher.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.WebDAV
{
    using System;
    using System.Globalization;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using NCloud.EndPoints.WebDAV.Configurations;
    using NWebDav.Server;
    using NWebDav.Server.AspNetCore;
    using NWebDav.Server.Helpers;
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
    private readonly WebDAVConfig webDAVConfig;

    /// <summary>
    /// Defines the dispatcher.
    /// </summary>
    private WebDavDispatcher dispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="NCloudWebDAVDispatcher"/> class.
    /// </summary>
    /// <param name="store">The store<see cref="IStore"/>.</param>
    /// <param name="logger">The logger<see cref="ILogger{NCloudWebDAVDispatcher}"/>.</param>
    public NCloudWebDAVDispatcher(IStore store, ILogger<NCloudWebDAVDispatcher> logger, WebDAVConfig webDAVConfig)
    {
        var requestHandlerFactory = new RequestHandlerFactory();
        this.dispatcher = new WebDavDispatcher(store, requestHandlerFactory);
        this.logger = logger;
        this.webDAVConfig = webDAVConfig;
    }

    /// <summary>
    /// The DispatchRequestAsync.
    /// </summary>
    /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
    /// <returns>The <see cref="Task"/>.</returns>
    public Task DispatchRequestAsync(IHttpContext httpContext)
    {
        if (webDAVConfig.Authentication)
        {
            var authorization = httpContext.Request.GetHeaderValue("Authorization");
            if (string.IsNullOrEmpty(authorization))
            {
                return HandlerUnauthorized(httpContext);
            }
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(authorization);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                var username = credentials[0];
                var password = credentials[1];
                if (username != webDAVConfig.UserName || password != webDAVConfig.Password)
                {
                    return HandlerUnauthorized(httpContext);
                }
            }
            catch (Exception)
            {
                return HandlerUnauthorized(httpContext);
            }
        }

        logger.LogDebug("DispatchRequestAsync url:{url}, method:{method}", httpContext.Request.Url, httpContext.Request.HttpMethod);
        return dispatcher.DispatchRequestAsync(httpContext);
    }

    private Task HandlerUnauthorized(IHttpContext httpContext)
    {
        httpContext.Response.SetHeaderValue("www-authenticate", "Basic realm = \"ncloud-webdav\"");
        httpContext.Response.SetHeaderValue("date", DateTime.Now.ToString("r", CultureInfo.GetCultureInfo("en-US")));
        httpContext.Response.SetStatus(DavStatusCode.Unauthorized);
        var ctx = ((AspNetCoreContext)httpContext).HttpContext;
        ctx.Response.WriteAsync("Authentication required");
        return Task.CompletedTask;
    }
}
}