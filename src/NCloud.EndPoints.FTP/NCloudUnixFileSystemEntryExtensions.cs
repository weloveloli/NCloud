// -----------------------------------------------------------------------
// <copyright file="NCloudUnixFileSystemEntryExtensions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP
{
    using NCloud.FileProviders.Support;

    /// <summary>
    /// Defines the <see cref="NCloudUnixFileSystemEntryExtensions" />.
    /// </summary>
    public static class NCloudUnixFileSystemEntryExtensions
    {
        /// <summary>
        /// The GetPath.
        /// </summary>
        /// <param name="path">The path<see cref="NCloudUnixFileSystemEntry"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetPath(this NCloudUnixFileSystemEntry path)
        {
            return path.InnerIFileInfo.GetVirtualOrPhysicalPath();
        }
    }
}
