// -----------------------------------------------------------------------
// <copyright file="NCloudFileInfoUnixFileEntry.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP
{
    using FubarDev.FtpServer.FileSystem;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="NCloudFileInfoUnixFileEntry" />.
    /// </summary>
    public class NCloudFileInfoUnixFileEntry : NCloudUnixFileSystemEntry, IUnixFileEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudFileInfoUnixFileEntry"/> class.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        public NCloudFileInfoUnixFileEntry(IFileInfo fileInfo) : base(fileInfo)
        {
        }

        /// <summary>
        /// Gets the Size.
        /// </summary>
        public long Size => this.InnerIFileInfo.Length;
    }
}
