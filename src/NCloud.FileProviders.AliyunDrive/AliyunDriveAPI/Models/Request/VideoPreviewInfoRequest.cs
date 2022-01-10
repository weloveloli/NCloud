// -----------------------------------------------------------------------
// <copyright file="VideoPreviewInfoRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request
{
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;

    /// <summary>
    /// Defines the <see cref="VideoPreviewInfoRequest" />.
    /// </summary>
    public class VideoPreviewInfoRequest : FileBaseRequest
    {
        /// <summary>
        /// Gets or sets the Category.
        /// </summary>
        public string Category { get; set; } = "live_transcoding";

        /// <summary>
        /// Gets or sets the TemplateId.
        /// </summary>
        public VideoPreviewTemplateType TemplateId { get; set; } = VideoPreviewTemplateType.NONE;
    }
}
