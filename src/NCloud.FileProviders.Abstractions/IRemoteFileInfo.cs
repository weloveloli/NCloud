// -----------------------------------------------------------------------
// <copyright file="IRemoteFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="IRemoteFileInfo" />.
    /// </summary>
    public interface IRemoteFileInfo : IFileInfo
    {
        /// <summary>
        /// Gets or sets the RemoteUrl.
        /// </summary>
        public string RemoteUrl { get; set; }
    }
}
