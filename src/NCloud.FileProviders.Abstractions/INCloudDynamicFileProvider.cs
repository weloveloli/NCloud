// -----------------------------------------------------------------------
// <copyright file="IFileProviderRegistration.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using System.Collections.Generic;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="INCloudDynamicFileProvider" />.
    /// </summary>
    public interface INCloudDynamicFileProvider : IFileProvider
    {
        /// <summary>
        /// The AddProvider.
        /// </summary>
        /// <param name="providers">The providers<see cref="BaseNCloudFileProvider[]"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool AddProvider(params BaseNCloudFileProvider[] providers);

        /// <summary>
        /// The RemoveProvider.
        /// </summary>
        /// <param name="config">The config<see cref="IEnumerable{string}"/>.</param>
        void RemoveProvider(IEnumerable<string> config);
    }
}
