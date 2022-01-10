// -----------------------------------------------------------------------
// <copyright file="VideoPreviewTemplateType.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the VideoPreviewTemplateType.
    /// </summary>
    public enum VideoPreviewTemplateType
    {
        /// <summary>
        /// Defines the NONE.
        /// </summary>
        [JsonPropertyName("")]
        NONE,

        /// <summary>
        /// Defines the SD.
        /// </summary>
        SD,

        /// <summary>
        /// Defines the HD.
        /// </summary>
        HD,

        /// <summary>
        /// Defines the FHD.
        /// </summary>
        FHD
    }
}
