// -----------------------------------------------------------------------
// <copyright file="FileType.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the FileType.
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Defines the Folder.
        /// </summary>
        [JsonPropertyName("folder")]
        Folder,

        /// <summary>
        /// Defines the File.
        /// </summary>
        [JsonPropertyName("file")]
        File
    }
}
