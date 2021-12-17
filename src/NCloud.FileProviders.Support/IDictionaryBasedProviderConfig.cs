// -----------------------------------------------------------------------
// <copyright file="IDictionaryBasedProviderConfig.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System.Collections.Generic;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="IDictionaryBasedProviderConfig" />.
    /// </summary>
    public interface IDictionaryBasedProviderConfig
    {
        /// <summary>
        /// The GetFileDict.
        /// </summary>
        /// <returns>The <see cref="IDictionary{String, IFileInfo}"/>.</returns>
        IEnumerable<(string key, IFileInfo info)> GetFileInfos();
    }
}
