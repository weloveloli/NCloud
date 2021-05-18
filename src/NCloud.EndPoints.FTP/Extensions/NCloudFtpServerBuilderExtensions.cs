// -----------------------------------------------------------------------
// <copyright file="NCloudFtpServerBuilderExtensions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP.Extensions
{
    using System;
    using FubarDev.FtpServer;
    using FubarDev.FtpServer.FileSystem;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using NCloud.EndPoints.FTP.Configurations;

    /// <summary>
    /// Extension methods for <see cref="IFtpServerBuilder"/>.
    /// </summary>
    public static class NCloudFtpServerBuilderExtensions
    {
        /// <summary>
        /// Uses the .NET file system API.
        /// </summary>
        /// <param name="builder">The builder<see cref="IFtpServerBuilder"/>.</param>
        /// <param name="ftpOptions">The ftpOptions<see cref="FtpOptions"/>.</param>
        /// <returns>The <see cref="IFtpServerBuilder"/>.</returns>
        public static IFtpServerBuilder UseNCloudFileSystem(this IFtpServerBuilder builder, FtpOptions ftpOptions)
        {
            return UseNCloudFileSystem<IFileProvider>(builder, ftpOptions);
        }

        /// <summary>
        /// Uses the .NET file system API.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="builder">The builder<see cref="IFtpServerBuilder"/>.</param>
        /// <param name="options">The options<see cref="FtpOptions"/>.</param>
        /// <returns>The <see cref="IFtpServerBuilder"/>.</returns>
        public static IFtpServerBuilder UseNCloudFileSystem<T>(this IFtpServerBuilder builder, FtpOptions options) where T : IFileProvider
        {
            if (options.Enable)
            {
                var service = builder.Services;
                if (options.Authentication == MembershipProviderType.Default)
                {
                     builder.EnableAnonymousAuthentication();
                }
                if ((options.Authentication & MembershipProviderType.Anonymous) != 0)
                {
                    builder.EnableAnonymousAuthentication();
                }
                service.AddSingleton<IFileSystemClassFactory, NCloudFileSystemClassFactory>((sp) => { return new NCloudFileSystemClassFactory(sp.GetService<T>()); });
                service.Configure<FubarDev.FtpServer.FtpServerOptions>(
                    opt =>
                    {
                        opt.ServerAddress = options.Server.Address;
                        opt.Port = options.GetServerPort();
                        opt.MaxActiveConnections = options.Server.MaxActiveConnections ?? 0;
                        opt.ConnectionInactivityCheckInterval =
                            TimeSpan.FromSeconds(options.Server.ConnectionInactivityCheckInterval ?? 60);
                    });
                service.Configure<SimplePasvOptions>((opt) =>
                {
                    var portRange = options.GetPasvPortRange();
                    if (portRange != null)
                    {
                        (opt.PasvMinPort, opt.PasvMaxPort) = portRange.Value;
                    }
                });
                service.Configure<PasvCommandOptions>(opt => opt.PromiscuousPasv = options.Server.Pasv.Promiscuous);
            }
            return builder;
        }
    }
}
