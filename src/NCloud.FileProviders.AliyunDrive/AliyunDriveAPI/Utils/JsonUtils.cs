// -----------------------------------------------------------------------
// <copyright file="JsonUtils.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Utils
{
    using System.Text.Json;

    /// <summary>
    /// Defines the <see cref="JsonUtils" />.
    /// </summary>
    internal static class JsonUtils
    {
        /// <summary>
        /// The Clone.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public static T Clone<T>(object obj)
        {
            string json = JsonSerializer.Serialize(obj, AliyunDriveApiClient.JsonSerializerOptions);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
