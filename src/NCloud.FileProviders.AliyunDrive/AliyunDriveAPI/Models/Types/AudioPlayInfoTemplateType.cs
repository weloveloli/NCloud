// -----------------------------------------------------------------------
// <copyright file="AudioPlayInfoTemplateType.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the AudioPlayInfoTemplateType.
    /// </summary>
    public enum AudioPlayInfoTemplateType
    {
        /// <summary>
        /// Defines the NONE.
        /// </summary>
        [JsonPropertyName("")]
        NONE,

        /// <summary>
        /// Defines the LQ.
        /// </summary>
        LQ,

        /// <summary>
        /// Defines the HQ.
        /// </summary>
        HQ
    }
}
