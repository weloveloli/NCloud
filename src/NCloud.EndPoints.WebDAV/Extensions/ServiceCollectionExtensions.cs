// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.WebDAV.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using NCloud.EndPoints.WebDAV.Configurations;

    /// <summary>
    /// Defines the <see cref="ServiceCollectionExtensions" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the FTP server services to the collection.
        /// </summary>
        /// <param name="services">The service collection to add the FTP server services to.</param>
        /// <param name="webDAVConfig">The webDAVConfig<see cref="WebDAVConfig"/>.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddNCloudWebDAVServer(
            this IServiceCollection services, WebDAVConfig webDAVConfig)
        {
            services.AddSingleton(webDAVConfig);
            return services;
        }
    }
}
