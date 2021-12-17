// -----------------------------------------------------------------------
// <copyright file="NCloudHostedWebDAVServer.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.WebDAV
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using NCloud.EndPoints.WebDAV.Configurations;
    using NWebDav.Server;
    using NWebDav.Server.Http;
    using NWebDav.Server.HttpListener;

    /// <summary>
    /// Defines the <see cref="NCloudHostedWebDAVServer" />.
    /// </summary>
    public class NCloudHostedWebDAVServer : IHostedService
    {
        /// <summary>
        /// Defines the httpListener.
        /// </summary>
        private HttpListener httpListener;

        /// <summary>
        /// Defines the cancellationTokenSource.
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Defines the webDavDispatcher.
        /// </summary>
        private readonly IWebDavDispatcher webDavDispatcher;

        /// <summary>
        /// Defines the webDAVConfig.
        /// </summary>
        private readonly WebDAVConfig webDAVConfig;


        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudHostedWebDAVServer"/> class.
        /// </summary>
        /// <param name="webDavDispatcher">The webDavDispatcher<see cref="WebDavDispatcher"/>.</param>
        public NCloudHostedWebDAVServer(IWebDavDispatcher webDavDispatcher, WebDAVConfig webDAVConfig, ILogger<NCloudHostedWebDAVServer> logger)
        {

            httpListener = new HttpListener();
            // Add the prefix
            httpListener.Prefixes.Add($"{webDAVConfig.Protocol}://{webDAVConfig.Ip}:{webDAVConfig.Port}/");
            if (webDAVConfig.Authentication)
            {
                // Check if HTTPS is enabled
                if (webDAVConfig.Protocol != "https")
                    logger.LogWarning("Most WebDAV clients cannot use authentication on a non-HTTPS connection");

                // Set the authentication scheme and realm
                httpListener.AuthenticationSchemes = AuthenticationSchemes.Basic;
                httpListener.Realm = "WebDAV server";
            }
            else
            {
                // Allow anonymous access
                httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            }

            this.webDavDispatcher = webDavDispatcher;
            this.webDAVConfig = webDAVConfig;
        }

        /// <summary>
        /// The StartAsync.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            // Start the HTTP listener
            httpListener.Start();

            // Determine the WebDAV username/password for authorization
            // (only when basic authentication is enabled)
            var webdavUsername = webDAVConfig.UserName ?? "test";
            var webdavPassword = webDAVConfig.Password ?? "test";

            HttpListenerContext httpListenerContext;
            while (!token.IsCancellationRequested && (httpListenerContext = await httpListener.GetContextAsync().ConfigureAwait(false)) != null)
            {
                // Determine the proper HTTP context
                IHttpContext httpContext;
                if (httpListenerContext.Request.IsAuthenticated)
                    httpContext = new HttpBasicContext(httpListenerContext, checkIdentity: i => i.Name == webdavUsername && i.Password == webdavPassword);
                else
                    httpContext = new HttpContext(httpListenerContext);

                // Dispatch the request
                await webDavDispatcher.DispatchRequestAsync(httpContext).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// The StopAsync.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }


    }
}
