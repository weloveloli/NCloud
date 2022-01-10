// -----------------------------------------------------------------------
// <copyright file="AudioPlayInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models
{
    using System.Text.Json.Serialization;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;

    /// <summary>
    /// Defines the <see cref="AudioPlayInfo" />.
    /// </summary>
    public class AudioPlayInfo
    {
        /// <summary>
        /// Gets or sets the TemplateId.
        /// </summary>
        public AudioPlayInfoTemplateType TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets a value indicating whether IsFinished.
        /// </summary>
        [JsonIgnore]
        public bool IsFinished => Status == "status";

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }
    }
}
