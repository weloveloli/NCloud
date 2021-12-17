// -----------------------------------------------------------------------
// <copyright file="VirtualProviderConfig.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Virtual
{
    using System.Collections.Generic;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.Support;

    /// <summary>
    /// Defines the <see cref="VirtualProviderConfig" />.
    /// </summary>
    public class VirtualProviderConfig : BaseProviderConfig, IEmbeddableProviderConfig, IDictionaryBasedProviderConfig
    {
        /// <summary>
        /// Gets or sets the FileSettings.
        /// </summary>
        public List<FileSetting> FileSettings { get; set; }

        /// <summary>
        /// Gets or sets the FileInfos.
        /// </summary>
        public IEnumerable<(string key, IFileInfo info)> FileInfos { get; set; }

        /// <summary>
        /// Gets the Type.
        /// </summary>
        public override string Type => "virtual";

        /// <summary>
        /// The IsLasyLoad.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsLasyLoad()
        {
            return true;
        }

        /// <summary>
        /// The GetFileInfos.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{(string key, IFileInfo info)}"/>.</returns>
        public IEnumerable<(string key, IFileInfo info)> GetFileInfos()
        {
            return FileInfos;
        }
    }
}
