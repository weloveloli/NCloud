// -----------------------------------------------------------------------
// <copyright file="VirtualInnerFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Virtual
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;

    /// <summary>
    /// Defines the <see cref="VirtualInnerFileProvider" />.
    /// </summary>
    public class VirtualInnerFileProvider : DictionaryBasedFileProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualInnerFileProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="files">The files<see cref="IDictionary{string, IFileInfo}"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        public VirtualInnerFileProvider(IServiceProvider provider, IDictionary<string, IFileInfo> files, string config, string prefix) : base(provider, config, prefix)
        {
            _files = files;
        }
    }
}
