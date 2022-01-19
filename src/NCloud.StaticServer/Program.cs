// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.StaticServer
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var webdavHost = CreateWebDavHostBuilder(args).Build();

            await Task.WhenAny(
                host.RunAsync(),
                webdavHost.RunAsync()
            );
        }

        /// <summary>
        /// The CreateHostBuilder.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<StaticStartup>();
                });

        public static IHostBuilder CreateWebDavHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args);
            host.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://*:11111").UseStartup<WebDAVStartup>();
            });

            return host;
        }
    }
}
