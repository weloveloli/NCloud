// -----------------------------------------------------------------------
// <copyright file="VideoPreviewPlayInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models
{
    using System;
    using System.Text.Json.Serialization;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;

    /// <summary>
    /// Defines the <see cref="VideoPreviewPlayInfo" />.
    /// </summary>
    public class VideoPreviewPlayInfo
    {
        /// <summary>
        /// Gets or sets the Category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the Meta.
        /// </summary>
        public VideoPreviewPlayInfoMeta Meta { get; set; }

        /// <summary>
        /// Gets or sets the LiveTranscodingTaskList.
        /// </summary>
        public LiveTranscodingTask[] LiveTranscodingTaskList { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="VideoPreviewPlayInfoMeta" />.
    /// </summary>
    public class VideoPreviewPlayInfoMeta
    {
        /// <summary>
        /// Gets or sets the Duration.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the Width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the Height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the LiveTranscodingMeta.
        /// </summary>
        public LiveTranscodingMeta LiveTranscodingMeta { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="LiveTranscodingMeta" />.
    /// </summary>
    public class LiveTranscodingMeta
    {
        /// <summary>
        /// Gets or sets the TsSegment.
        /// </summary>
        public int TsSegment { get; set; }

        /// <summary>
        /// Gets or sets the TsTotalCount.
        /// </summary>
        public int TsTotalCount { get; set; }

        /// <summary>
        /// Gets or sets the TsPreCount.
        /// </summary>
        public int TsPreCount { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="LiveTranscodingTask" />.
    /// </summary>
    public class LiveTranscodingTask
    {
        /// <summary>
        /// Gets or sets the TemplateId.
        /// </summary>
        public VideoPreviewTemplateType TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the TemplateName.
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the Stage.
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets a value indicating whether IsFinished.
        /// </summary>
        [JsonIgnore]
        public bool IsFinished => Status == "finished";
    }
}
