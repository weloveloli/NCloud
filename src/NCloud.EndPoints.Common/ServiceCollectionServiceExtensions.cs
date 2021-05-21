// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionServiceExtensions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection
{
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.Support;

    /// <summary>
    /// Defines the <see cref="ServiceCollectionServiceExtensions" />.
    /// </summary>
    public static class ServiceCollectionServiceExtensions
    {
        /// <summary>
        /// The AddNCloudFileProviders.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddNCloudFileProviders(this IServiceCollection services)
        {
            var registry = new DefaultNCloudFileProviderRegistry();
            services.AddSingleton<INCloudFileProviderRegistry>(registry);
            services.AddSingleton((sp)=>(IFileProvider)sp.GetService(typeof(INCloudFileProviderRegistry)));
            return services;
        }
    }
}
