// -----------------------------------------------------------------------
// <copyright file="FileSearchRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request
{
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Utils;

    /// <summary>
    /// Defines the <see cref="FileSearchRequest" />.
    /// </summary>
    public class FileSearchRequest
    {
        /// <summary>
        /// Gets or sets the DriveId.
        /// </summary>
        public string DriveId { get; set; }

        /// <summary>
        /// Gets or sets the Limit.
        /// </summary>
        public int? Limit { get; set; } = 100;

        /// <summary>
        /// Gets or sets the Marker.
        /// </summary>
        public string Marker { get; set; }

        /// <summary>
        /// Gets or sets the Query.
        /// </summary>
        public string Query { get; set; }

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
        /// Gets or sets the OrderByType.
        /// </summary>
        public OrderByType? OrderByType { get; set; } = Types.OrderByType.UpdatedAt;

        /// <summary>
        /// Gets or sets the OrderDirection.
        /// </summary>
        public OrderDirectionType? OrderDirection { get; set; } = OrderDirectionType.DESC;

        /// <summary>
        /// Gets the OrderBy.
        /// </summary>
        public string OrderBy => OrderByType.HasValue && OrderDirection.HasValue
            ? ReflectionUtils.GetEnumValueName(OrderByType.Value) + " " + ReflectionUtils.GetEnumValueName(OrderDirection.Value) : null;
    }
}
