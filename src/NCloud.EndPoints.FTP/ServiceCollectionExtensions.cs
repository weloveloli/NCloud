// -----------------------------------------------------------------------
// <copyright file="Class1.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection
{
    using FubarDev.FtpServer;
    using Microsoft.Extensions.FileProviders;
    using NCloud.EndPoints.FTP;

    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the FTP server services to the collection.
        /// </summary>
        /// <param name="services">The service collection to add the FTP server services to.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddNCloudFtpServer(
            this IServiceCollection services)
        {
            return AddNCloudFtpServer<IFileProvider>(services);
        }

        /// <summary>
        /// Adds the FTP server services to the collection.
        /// </summary>
        /// <param name="services">The service collection to add the FTP server services to.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddNCloudFtpServer<T>(
            this IServiceCollection services) where T : IFileProvider
        {
            // Add FTP server services
            // DotNetFileSystemProvider = Use the .NET file system functionality
            // AnonymousMembershipProvider = allow only anonymous logins
            services.AddFtpServer(builder => builder
                .UseNCloudFileSystem<T>() // Use the NCloud file system functionality
                .EnableAnonymousAuthentication()); // allow anonymous logins

            return services;
        }
    }
}
