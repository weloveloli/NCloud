// -----------------------------------------------------------------------
// <copyright file="NCloudUnixDirectoryEntry.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP
{
    using FubarDev.FtpServer.FileSystem;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Support;

    /// <summary>
    /// Defines the <see cref="NCloudUnixDirectoryEntry" />.
    /// </summary>
    public class NCloudUnixDirectoryEntry : NCloudUnixFileSystemEntry, IUnixDirectoryEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudUnixDirectoryEntry"/> class.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        public NCloudUnixDirectoryEntry(IFileInfo fileInfo) : base(fileInfo)
        {
        }

        /// <summary>
        /// Gets a value indicating whether IsRoot.
        /// </summary>
        public bool IsRoot => this.InnerIFileInfo.IsDirectory && this.InnerIFileInfo.GetVirtualOrPhysicalPath() == "/";

        /// <summary>
        /// Gets a value indicating whether IsDeletable.
        /// </summary>
        public bool IsDeletable => false;
    }
}
