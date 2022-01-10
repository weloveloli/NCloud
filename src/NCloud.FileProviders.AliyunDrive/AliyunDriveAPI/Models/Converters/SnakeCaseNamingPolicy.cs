// -----------------------------------------------------------------------
// <copyright file="SnakeCaseNamingPolicy.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Converters
{
    using System.Linq;
    using System.Text.Json;

    /// <summary>
    /// Defines the <see cref="SnakeCaseNamingPolicy" />.
    /// </summary>
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        /// <summary>
        /// The ConvertName.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ConvertName(string name)
            => string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + char.ToLowerInvariant(x).ToString() : char.ToLowerInvariant(x).ToString()));
    }
}
