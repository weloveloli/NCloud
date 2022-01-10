// -----------------------------------------------------------------------
// <copyright file="FileMoveRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request
{
    /// <summary>
    /// Defines the <see cref="FileMoveRequest" />.
    /// </summary>
    public class FileMoveRequest : FileBaseRequest
    {
        /// <summary>
        /// Gets or sets the ToDriveId.
        /// </summary>
        public string ToDriveId { get; set; }

        /// <summary>
        /// Gets or sets the ToParentFileId.
        /// </summary>
        public string ToParentFileId { get; set; }
    }
}
