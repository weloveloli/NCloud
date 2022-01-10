// -----------------------------------------------------------------------
// <copyright file="PreUploadResponse.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response
{
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;

    /// <summary>
    /// Defines the <see cref="PreUploadResponse" />.
    /// </summary>
    public class PreUploadResponse
    {
        /// <summary>
        /// Gets or sets the ParentFileId.
        /// </summary>
        public string ParentFileId { get; set; }

        /// <summary>
        /// Gets or sets the PartInfoList.
        /// </summary>
        public FileUploadPartInfoWithUrl[] PartInfoList { get; set; }

        /// <summary>
        /// Gets or sets the UploadId.
        /// </summary>
        public string UploadId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether RepidUpload.
        /// </summary>
        public bool RepidUpload { get; set; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public FileType Type { get; set; }

        /// <summary>
        /// Gets or sets the FileId.
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Gets or sets the DomainId.
        /// </summary>
        public string DomainId { get; set; }

        /// <summary>
        /// Gets or sets the DriveId.
        /// </summary>
        public string DriveId { get; set; }

        /// <summary>
        /// Gets or sets the FileName.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the EncryptMode.
        /// </summary>
        public string EncryptMode { get; set; }

        /// <summary>
        /// Gets or sets the Location.
        /// </summary>
        public string Location { get; set; }
    }
}
