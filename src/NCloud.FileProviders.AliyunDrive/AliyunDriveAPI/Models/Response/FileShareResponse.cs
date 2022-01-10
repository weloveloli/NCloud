// -----------------------------------------------------------------------
// <copyright file="FileShareResponse.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="FileShareResponse" />.
    /// </summary>
    public class FileShareResponse
    {
        /// <summary>
        /// Gets or sets the CreatedAt.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the Creator.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the DownloadCount.
        /// </summary>
        public int DownloadCount { get; set; }

        /// <summary>
        /// Gets or sets the DriveId.
        /// </summary>
        public string DriveId { get; set; }

        /// <summary>
        /// Gets or sets the Expiration.
        /// </summary>
        public DateTime? Expiration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Expired.
        /// </summary>
        public bool Expired { get; set; }

        /// <summary>
        /// Gets or sets the FileId.
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Gets or sets the FileIdList.
        /// </summary>
        public List<string> FileIdList { get; set; }

        /// <summary>
        /// Gets or sets the PreviewCount.
        /// </summary>
        public int PreviewCount { get; set; }

        /// <summary>
        /// Gets or sets the SaveCount.
        /// </summary>
        public int SaveCount { get; set; }

        /// <summary>
        /// Gets or sets the ShareId.
        /// </summary>
        public string ShareId { get; set; }

        /// <summary>
        /// Gets or sets the ShareMsg.
        /// </summary>
        public string ShareMsg { get; set; }

        /// <summary>
        /// Gets or sets the ShareName.
        /// </summary>
        public string ShareName { get; set; }

        /// <summary>
        /// Gets or sets the SharePolicy.
        /// </summary>
        public string SharePolicy { get; set; }

        /// <summary>
        /// Gets or sets the SharePwd.
        /// </summary>
        public string SharePwd { get; set; }

        /// <summary>
        /// Gets or sets the ShareUrl.
        /// </summary>
        public string ShareUrl { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedAt.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
