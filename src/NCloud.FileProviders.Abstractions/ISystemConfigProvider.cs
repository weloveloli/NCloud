// -----------------------------------------------------------------------
// <copyright file="ISystemConfigProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    /// <summary>
    /// Defines the <see cref="ISystemConfigProvider" />.
    /// </summary>
    public interface ISystemConfigProvider
    {
        /// <summary>
        /// Gets the ConfigFolder.
        /// </summary>
        public string ConfigFolder { get; }

        /// <summary>
        /// Gets the CacheFolder.
        /// </summary>
        public string CacheFolder { get; }
    }
}
