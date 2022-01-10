// -----------------------------------------------------------------------
// <copyright file="RateLimit.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models
{
    /// <summary>
    /// Defines the <see cref="RateLimit" />.
    /// </summary>
    public class RateLimit
    {
        /// <summary>
        /// Gets or sets the PartSpeed.
        /// </summary>
        public long PartSpeed { get; set; }

        /// <summary>
        /// Gets or sets the PartSize.
        /// </summary>
        public long PartSize { get; set; }
    }
}
