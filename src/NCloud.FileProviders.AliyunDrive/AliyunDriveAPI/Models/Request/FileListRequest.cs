// -----------------------------------------------------------------------
// <copyright file="FileListRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request
{
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;

    /// <summary>
    /// Defines the <see cref="FileListRequest" />.
    /// </summary>
    public class FileListRequest
    {
        /// <summary>
        /// Gets or sets the DriveId.
        /// </summary>
        public string DriveId { get; set; }

        /// <summary>
        /// Gets or sets the ParentFileId.
        /// </summary>
        public string ParentFileId { get; set; } = "root";

        /// <summary>
        /// Gets or sets the Limit.
        /// </summary>
        public int? Limit { get; set; } = 100;

        /// <summary>
        /// Gets or sets the Marker.
        /// </summary>
        public string Marker { get; set; }

        /// <summary>
        /// Gets or sets the All.
        /// </summary>
        public bool? All { get; set; } = false;

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

        /// <summary>
        /// Gets or sets the Fields.
        /// </summary>
        public string Fields { get; set; } = "*";

        /// <summary>
        /// Gets or sets the OrderBy.
        /// </summary>
        public OrderByType? OrderBy { get; set; } = OrderByType.UpdatedAt;

        /// <summary>
        /// Gets or sets the OrderDirection.
        /// </summary>
        public OrderDirectionType? OrderDirection { get; set; } = OrderDirectionType.DESC;
    }
}
