// -----------------------------------------------------------------------
// <copyright file="ClearRecyclebinResponse.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response
{
    /// <summary>
    /// Defines the <see cref="ClearRecyclebinResponse" />.
    /// </summary>
    public class ClearRecyclebinResponse
    {
        /// <summary>
        /// Gets or sets the DomainId.
        /// </summary>
        public string DomainId { get; set; }

        /// <summary>
        /// Gets or sets the DriveId.
        /// </summary>
        public string DriveId { get; set; }

        /// <summary>
        /// Gets or sets the TaskId.
        /// </summary>
        public string TaskId { get; set; }
    }
}
