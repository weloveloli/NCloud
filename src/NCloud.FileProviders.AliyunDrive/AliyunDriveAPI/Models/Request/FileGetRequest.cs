// -----------------------------------------------------------------------
// <copyright file="FileGetRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request
{
    /// <summary>
    /// Defines the <see cref="FileGetRequest" />.
    /// </summary>
    public class FileGetRequest : FileBaseRequest
    {
        /// <summary>
        /// Gets or sets the UrlExpireSec.
        /// </summary>
        public int? UrlExpireSec { get; set; } = 1600;

        /// <summary>
        /// Gets or sets the ImageThumbnailProcess.
        /// </summary>
        public string ImageThumbnailProcess { get; set; } = "image/resize,w_400/format,jpeg";

        /// <summary>
        /// Gets or sets the ImageUrlProcess.
        /// </summary>
        public string ImageUrlProcess { get; set; } = "image/resize,w_1920/format,jpeg";

        /// <summary>
        /// Gets or sets the VideoThumbnailProcess.
        /// </summary>
        public string VideoThumbnailProcess { get; set; } = "video/snapshot,t_0,f_jpg,ar_auto,w_300";
    }
}
