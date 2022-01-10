// -----------------------------------------------------------------------
// <copyright file="FileListResponse.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response
{
    /// <summary>
    /// Defines the <see cref="FileListResponse" />.
    /// </summary>
    public class FileListResponse
    {
        /// <summary>
        /// Gets or sets the Items.
        /// </summary>
        public FileItem[] Items { get; set; }

        /// <summary>
        /// Gets or sets the NextMarker.
        /// </summary>
        public string NextMarker { get; set; }

        /// <summary>
        /// Gets or sets the PunishedFileCount.
        /// </summary>
        public int PunishedFileCount { get; set; }
    }
}
