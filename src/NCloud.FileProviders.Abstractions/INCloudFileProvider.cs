// -----------------------------------------------------------------------
// <copyright file="INCloudFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="INCloudFileProvider" />.
    /// </summary>
    public interface INCloudFileProvider : IFileProvider
    {
        /// <summary>
        /// The Hash.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string Key { get; }
    }
}
