// -----------------------------------------------------------------------
// <copyright file="IEmbeddableProviderConfig.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    /// <summary>
    /// Defines the <see cref="IEmbeddableProviderConfig" />.
    /// </summary>
    public interface IEmbeddableProviderConfig
    {
        /// <summary>
        /// The IsLasyLoad.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsLasyLoad();
    }
}
