// -----------------------------------------------------------------------
// <copyright file="CreateFolderResponse.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response
{
    using System.Text.Json.Serialization;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;

    /// <summary>
    /// Defines the <see cref="CreateFolderResponse" />.
    /// </summary>
    public class CreateFolderResponse
    {
        /// <summary>
        /// Gets or sets the ParentFileId.
        /// </summary>
        public string ParentFileId { get; set; }

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
        /// Gets or sets the Status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets a value indicating whether IsAvailable.
        /// </summary>
        [JsonIgnore]
        public bool IsAvailable => Status == "available";

        /// <summary>
        /// Gets or sets the Exist.
        /// </summary>
        public bool? Exist { get; set; }
    }
}
