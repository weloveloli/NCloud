// -----------------------------------------------------------------------
// <copyright file="QueryConditionType.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the QueryConditionType.
    /// </summary>
    public enum QueryConditionType
    {
        /// <summary>
        /// Defines the Match.
        /// </summary>
        [JsonPropertyName("match")]
        Match,

        /// <summary>
        /// Defines the Equal.
        /// </summary>
        [JsonPropertyName("=")]
        Equal,

        /// <summary>
        /// Defines the GT.
        /// </summary>
        [JsonPropertyName(">")]
        GT,

        /// <summary>
        /// Defines the GE.
        /// </summary>
        [JsonPropertyName(">=")]
        GE,

        /// <summary>
        /// Defines the LT.
        /// </summary>
        [JsonPropertyName("<")]
        LT,

        /// <summary>
        /// Defines the LE.
        /// </summary>
        [JsonPropertyName("<=")]
        LE
    }
}
