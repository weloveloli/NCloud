// -----------------------------------------------------------------------
// <copyright file="Class1.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection
{
    using Microsoft.Extensions.FileProviders;
    using NCloud.EndPoints.FTP.Configurations;
    using NCloud.EndPoints.FTP.Extensions;

    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the FTP server services to the collection.
        /// </summary>
        /// <param name="services">The service collection to add the FTP server services to.</param>
        /// <param name="ftpOptions">The ftpOptions<see cref="FtpOptions"/>.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddNCloudFtpServer(
            this IServiceCollection services, FtpOptions ftpOptions)
        {
            return AddNCloudFtpServer<IFileProvider>(services, ftpOptions);
        }

        /// <summary>
        /// Adds the FTP server services to the collection.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="services">The service collection to add the FTP server services to.</param>
        /// <param name="ftpOptions">The ftpOptions<see cref="FtpOptions"/>.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddNCloudFtpServer<T>(
            this IServiceCollection services, FtpOptions ftpOptions) where T : IFileProvider
        {
            // Add FTP server services
            // DotNetFileSystemProvider = Use the .NET file system functionality
            // AnonymousMembershipProvider = allow only anonymous logins
            services.AddFtpServer(builder => builder.UseNCloudFileSystem<T>(ftpOptions));
            return services;
        }
    }
}
