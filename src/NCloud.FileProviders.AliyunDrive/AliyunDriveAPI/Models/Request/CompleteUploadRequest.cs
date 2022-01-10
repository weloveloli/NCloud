// -----------------------------------------------------------------------
// <copyright file="CompleteUploadRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request
{
    /// <summary>
    /// Defines the <see cref="CompleteUploadRequest" />.
    /// </summary>
    public class CompleteUploadRequest : FileBaseRequest
    {
        /// <summary>
        /// Gets or sets the UploadId.
        /// </summary>
        public string UploadId { get; set; }
    }
}
