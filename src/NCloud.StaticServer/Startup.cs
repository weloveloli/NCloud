// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.StaticServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NCloud.Core;
    using NCloud.EndPoints.FTP;
    using NCloud.EndPoints.Static;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.GitHub;
    using NCloud.FileProviders.Support;
    using NCloud.FileProviders.Support.Logger;
    using NCloud.StaticServer.Configuration;
    using Newtonsoft.Json;
    using Serilog;

    /// <summary>
    /// Defines the <see cref="Startup" />.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Defines the ncloud.
        /// </summary>
        protected NCloudStaticServerOptions ncloud;

        /// <summary>
        /// The ConfigureServices.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            this.ncloud = Configuration.GetSection("NCloud").Get<NCloudStaticServerOptions>();
            services.AddHttpClient();
            services.AddMemoryCache((cacheOption) =>
            {
                cacheOption.SizeLimit = 4096;
                cacheOption.ExpirationScanFrequency = TimeSpan.FromMinutes(5);
            });
            services.AddSingleton<GitHubClient>();
            services.AddSingleton<INCloudFileProviderRegistry, DefaultNCloudFileProviderRegistry>();
            services.AddSingleton<INCloudFileProvider>(s => s.GetService<INCloudFileProviderRegistry>());
            services.AddSingleton<INCloudFileProviderFactory, DefaultNCloudFileProviderFactory>();
            services.AddSingleton<IContentTypeProvider, MimeContentTypeProvider>();
            services.AddSingleton<ISystemConfigProvider>(ncloud);
            var startUps = Assembly.GetExecutingAssembly().GetProviderAssembly().SelectMany(e => e.GetExportedTypes())
                .Where(e => typeof(INCloudStartUp).IsAssignableFrom(e)).Select(t=>(INCloudStartUp)Activator.CreateInstance(t));
            foreach (var startup in startUps)
            {
                startup.ConfigureServices(services);
            }
            services.AddDirectoryBrowser();
            if (ncloud.FtpEnable)
            {
                services.AddNCloudFtpServer<INCloudFileProviderRegistry>(ncloud.Ftp).AddHostedService<NCloudHostedFtpService>();
            }
        }

        /// <summary>
        /// The Configure.
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The env<see cref="IWebHostEnvironment"/>.</param>
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            ApplicationLogging.LoggerFactory = loggerFactory;
            var service = app.ApplicationServices;
            var logger = service.GetService<ILogger<Startup>>();
            var fileProvider = service.GetService<INCloudFileProviderFactory>();
            var dynamicFileProvider = service.GetService<INCloudFileProviderRegistry>();
            List<BaseProviderConfig> stores;
            string storeConfig;
            var storeFile = ncloud.StoreFile;
            if (!string.IsNullOrWhiteSpace(storeFile) && File.Exists(storeFile))
            {
                logger.LogInformation($"use storefile {storeFile}");
                storeConfig = File.ReadAllText(storeFile);
            }
            else
            {
                storeConfig = JsonConvert.SerializeObject(Configuration.GetSection("Store").Get<List<Dictionary<string, string>>>());
            }
            stores = JsonConvert.DeserializeObject<List<BaseProviderConfig>>(storeConfig, new ProviderConfigConverter());
            if (stores != null)
            {
                foreach (var store in stores)
                {
                    if (store != null)
                    {
                        logger.LogInformation($"Enable {JsonConvert.SerializeObject(store)}");
                        dynamicFileProvider.AddProvider(fileProvider.CreateProvider(store));
                    }
                }
            }
            // This will make the HTTP requests log as rich logs instead of plain text.
            app.UseSerilogRequestLogging(); // <-- Add this line
        }
    }
}
