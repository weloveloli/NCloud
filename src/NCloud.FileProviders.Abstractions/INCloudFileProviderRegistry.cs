// -----------------------------------------------------------------------
// <copyright file="INCloudFileProviderRegistry.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using System.Collections.Generic;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="INCloudFileProviderRegistry" />.
    /// </summary>
    public interface INCloudFileProviderRegistry : IFileProvider
    {
        /// <summary>
        /// The AddProvider.
        /// </summary>
        /// <param name="providers">The providers.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool AddProvider(params INCloudFileProvider[] providers);

        /// <summary>
        /// The RemoveProvider.
        /// </summary>
        /// <param name="config">The config<see cref="IEnumerable{String}"/>.</param>
        void RemoveProvider(IEnumerable<string> config);
    }
}
