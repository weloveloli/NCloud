// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.StaticServer
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NCloud.EndPoints.FTP;
    using NCloud.EndPoints.Static;
    using NCloud.EndPoints.WebDAV;
    using NCloud.EndPoints.WebDAV.Extensions;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.GitHub;
    using NCloud.FileProviders.Support;
    using NCloud.StaticServer.Configuration;
    using Newtonsoft.Json;

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
        /// The ConfigureServices.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var ncloud = Configuration.GetSection("NCloud").Get<NCloudStaticServerOptions>();
            services.AddHttpClient();
            services.AddSingleton<GitHubClient>();
            services.AddSingleton<INCloudFileProviderRegistry, DefaultNCloudFileProviderRegistry>();
            services.AddSingleton<INCloudFileProvider>(s => s.GetService<INCloudFileProviderRegistry>());
            services.AddSingleton<INCloudFileProviderFactory, DefaultNCloudFileProviderFactory>();
            services.AddSingleton<IContentTypeProvider, MimeContentTypeProvider>();
            services.AddDirectoryBrowser();
            if (ncloud.FtpEnable)
            {
                services.AddNCloudFtpServer<INCloudFileProviderRegistry>(ncloud.Ftp).AddHostedService<NCloudHostedFtpService>();
            }
            if (ncloud.WebDAVEnable)
            {
                services.AddNCloudWebDAVServer(ncloud.WebDAVConfig).AddHostedService<NCloudHostedWebDAVServer>();
            }
        }

        /// <summary>
        /// The Configure.
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The env<see cref="IWebHostEnvironment"/>.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            var service = app.ApplicationServices;
            var fileProvider = service.GetService<INCloudFileProviderFactory>();
            var dynamicFileProvider = service.GetService<INCloudFileProviderRegistry>();
            var storeConfig = Configuration.GetSection("Store").Get<List<Dictionary<string,string>>>();
            var stores = JsonConvert.DeserializeObject<List<BaseProviderConfig>>(JsonConvert.SerializeObject(storeConfig), new ProviderConfigConverter());
            foreach (var store in stores)
            {
                dynamicFileProvider.AddProvider(fileProvider.CreateProvider(store));
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = dynamicFileProvider,
                RequestPath = "",
                DefaultContentType = "application/octet-stream",
                ServeUnknownFileTypes = false,
                ContentTypeProvider = service.GetService<IContentTypeProvider>()
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = dynamicFileProvider,
                RequestPath = ""
            });

        }
    }
}
