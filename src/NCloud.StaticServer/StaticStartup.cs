// -----------------------------------------------------------------------
// <copyright file="StaticStartup.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.StaticServer
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using NCloud.FileProviders.Abstractions;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.Hosting;

    public class StaticStartup : Startup
    {
        public StaticStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
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
            var dynamicFileProvider = service.GetService<INCloudFileProviderRegistry>();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = dynamicFileProvider,
                RequestPath = "/static",
                DefaultContentType = "application/octet-stream",
                ServeUnknownFileTypes = false,
                ContentTypeProvider = service.GetService<IContentTypeProvider>()
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = dynamicFileProvider,
                RequestPath = "/static"
            });
        }
    }
}
