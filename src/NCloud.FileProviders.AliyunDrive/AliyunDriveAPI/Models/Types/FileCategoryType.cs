// -----------------------------------------------------------------------
// <copyright file="FileCategoryType.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the FileCategoryType.
    /// </summary>
    public enum FileCategoryType
    {
        /// <summary>
        /// Defines the Image.
        /// </summary>
        [JsonPropertyName("image")]
        Image,

        /// <summary>
        /// Defines the Video.
        /// </summary>
        [JsonPropertyName("video")]
        Video,

        /// <summary>
        /// Defines the Audio.
        /// </summary>
        [JsonPropertyName("audio")]
        Audio,

        /// <summary>
        /// Defines the APP.
        /// </summary>
        [JsonPropertyName("app")]
        APP,

        /// <summary>
        /// Defines the Doc.
        /// </summary>
        [JsonPropertyName("doc")]
        Doc,

        /// <summary>
        /// Defines the Zip.
        /// </summary>
        [JsonPropertyName("zip")]
        Zip,

        /// <summary>
        /// Defines the Others.
        /// </summary>
        [JsonPropertyName("others")]
        Others
    }
}
