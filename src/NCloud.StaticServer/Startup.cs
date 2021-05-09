// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.StaticServer
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Rewrite;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.Abstractions.Extensions;
    using NCloud.FileProviders.GitHub;
    using NCloud.ServerCommon;

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
            services.AddHttpClient();
            services.AddSingleton<GitHubClient>();
            services.AddSingleton((sp) =>
            {
                var factory = new DefaultNCloudFileProviderFactory(sp);
                return factory;
            });
            services.AddSingleton<IContentTypeProvider, MimeContentTypeProvider>();
            services.AddDirectoryBrowser();
            services.AddControllersWithViews();
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
            var fileProvider = service.GetService<DefaultNCloudFileProviderFactory>();
            fileProvider.CreateProvider("github:weloveloli/NCloud", "/github");
            fileProvider.CreateProvider($"fs:{env.WebRootPath.ToPosixPath()}", "");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider,
                RequestPath = "",
                DefaultContentType = "application/octet-stream",
                ServeUnknownFileTypes = false,
                ContentTypeProvider = service.GetService<IContentTypeProvider>()
        });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = fileProvider,
                RequestPath = ""
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
