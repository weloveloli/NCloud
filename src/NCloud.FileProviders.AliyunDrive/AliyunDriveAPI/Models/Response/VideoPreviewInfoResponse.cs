// -----------------------------------------------------------------------
// <copyright file="VideoPreviewInfoResponse.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response
{
    /// <summary>
    /// Defines the <see cref="VideoPreviewInfoResponse" />.
    /// </summary>
    public class VideoPreviewInfoResponse
    {
        /// <summary>
        /// Gets or sets the DomainId.
        /// </summary>
        public string DomainId { get; set; }

        /// <summary>
        /// Gets or sets the DriveId.
        /// </summary>
        public string DriveId { get; set; }

        /// <summary>
        /// Gets or sets the FileId.
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Gets or sets the VideoPreviewPlayInfo.
        /// </summary>
        public VideoPreviewPlayInfo VideoPreviewPlayInfo { get; set; }
    }
}
