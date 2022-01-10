// -----------------------------------------------------------------------
// <copyright file="CheckNameModeType.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the CheckNameModeType.
    /// </summary>
    public enum CheckNameModeType
    {
        /// <summary>
        /// Defines the Refuse.
        /// </summary>
        [JsonPropertyName("refuse")]
        Refuse,

        /// <summary>
        /// Defines the AutoRename.
        /// </summary>
        [JsonPropertyName("auto_rename")]
        AutoRename,

        /// <summary>
        /// Defines the Overwrite.
        /// </summary>
        [JsonPropertyName("overwrite")]
        Overwrite
    }
}
