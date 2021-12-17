// -----------------------------------------------------------------------
// <copyright file="IFileProviderFactory.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="INCloudFileProviderFactory" />.
    /// </summary>
    public interface INCloudFileProviderFactory
    {
        /// <summary>
        /// CreateProvider helps creating fileProvider base on the configuration.
        /// </summary>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileProvider"/>.</returns>
        INCloudFileProvider CreateProvider(BaseProviderConfig config);

    }
}
