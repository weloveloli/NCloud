// -----------------------------------------------------------------------
// <copyright file="QueryRelationType.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the QueryRelationType.
    /// </summary>
    public enum QueryRelationType
    {
        /// <summary>
        /// Defines the AND.
        /// </summary>
        [JsonPropertyName("and")]
        AND,

        /// <summary>
        /// Defines the OR.
        /// </summary>
        [JsonPropertyName("or")]
        OR
    }
}
