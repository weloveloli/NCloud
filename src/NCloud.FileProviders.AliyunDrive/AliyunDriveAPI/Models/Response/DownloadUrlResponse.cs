// -----------------------------------------------------------------------
// <copyright file="DownloadUrlResponse.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response
{
    using System;

    /// <summary>
    /// Defines the <see cref="DownloadUrlResponse" />.
    /// </summary>
    public class DownloadUrlResponse
    {
        /// <summary>
        /// Gets or sets the Method.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the InternalUrl.
        /// </summary>
        public string InternalUrl { get; set; }

        /// <summary>
        /// Gets or sets the Expiration.
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Gets or sets the Size.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the RateLimit.
        /// </summary>
        public RateLimit RateLimit { get; set; }

        /// <summary>
        /// Gets or sets the Crc64Hash.
        /// </summary>
        public string Crc64Hash { get; set; }

        /// <summary>
        /// Gets or sets the ContentHash.
        /// </summary>
        public string ContentHash { get; set; }

        /// <summary>
        /// Gets or sets the ContentHashName.
        /// </summary>
        public string ContentHashName { get; set; }
    }
}
