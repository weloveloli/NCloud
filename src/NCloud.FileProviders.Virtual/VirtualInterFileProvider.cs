// -----------------------------------------------------------------------
// <copyright file="VirtualInterFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Virtual
{
    using System;
    using NCloud.FileProviders.Support;

    /// <summary>
    /// Defines the <see cref="VirtualInterFileProvider" />.
    /// </summary>
    internal class VirtualInterFileProvider : DictionaryBasedFileProvider<VirtualProviderConfig>
    {
        public VirtualInterFileProvider(IServiceProvider provider, VirtualProviderConfig config) : base(provider, config)
        {
        }
    }
}
