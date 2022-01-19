// -----------------------------------------------------------------------
// <copyright file="WebDAVStartup.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.StaticServer
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NCloud.EndPoints.WebDAV;
    using NCloud.EndPoints.WebDAV.Logging;
    using NWebDav.Server;
    using NWebDav.Server.AspNetCore;
    using NWebDav.Server.Stores;

    public class WebDAVStartup : Startup
    {
        public WebDAVStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSingleton(ncloud.WebDAVConfig);
            services.AddSingleton<IStore, NCloudStore>();
            services.AddSingleton<IWebDavDispatcher, NCloudWebDAVDispatcher>();
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
            NWebDav.Server.Logging.LoggerFactory.Factory = new WebDavLoggerFactory();
            var service = app.ApplicationServices;
            var webDavDispatcher = service.GetService<IWebDavDispatcher>();
            app.Run(async context =>
            {
                // Create the proper HTTP context
                var httpContext = new AspNetCoreContext(context);

                // Dispatch request
                await webDavDispatcher.DispatchRequestAsync(httpContext).ConfigureAwait(false);
            });
        }
    }
}
