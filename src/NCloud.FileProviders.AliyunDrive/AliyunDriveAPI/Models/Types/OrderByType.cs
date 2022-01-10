// -----------------------------------------------------------------------
// <copyright file="OrderByType.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the OrderByType.
    /// </summary>
    public enum OrderByType
    {
        /// <summary>
        /// Defines the Name.
        /// </summary>
        [JsonPropertyName("name")]
        Name,

        /// <summary>
        /// Defines the Size.
        /// </summary>
        [JsonPropertyName("size")]
        Size,

        /// <summary>
        /// Defines the UpdatedAt.
        /// </summary>
        [JsonPropertyName("updated_at")]
        UpdatedAt,

        /// <summary>
        /// Defines the CreatedAt.
        /// </summary>
        [JsonPropertyName("created_at")]
        CreatedAt,

        /// <summary>
        /// Defines the CustomField_1.
        /// </summary>
        [JsonPropertyName("custom_field_1")]
        CustomField_1,

        /// <summary>
        /// Defines the CustomField_2.
        /// </summary>
        [JsonPropertyName("custom_field_2")]
        CustomField_2
    }
}
