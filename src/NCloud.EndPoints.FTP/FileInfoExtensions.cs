// -----------------------------------------------------------------------
// <copyright file="FileInfoExtensions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP
{
    using FubarDev.FtpServer.FileSystem;
    using Microsoft.Extensions.FileProviders;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="FileInfoExtensions" />.
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// The ToEntry.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <returns>The <see cref="NCloudUnixFileSystemEntry"/>.</returns>
        public static IUnixFileSystemEntry ToEntry(this IFileInfo fileInfo)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));
            return fileInfo.IsDirectory ? new NCloudUnixDirectoryEntry(fileInfo) : new NCloudFileInfoUnixFileEntry(fileInfo);
        }
    }
}
