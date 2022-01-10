// -----------------------------------------------------------------------
// <copyright file="PreUploadRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request
{
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;

    /// <summary>
    /// Defines the <see cref="PreUploadRequest" />.
    /// </summary>
    public class PreUploadRequest
    {
        /// <summary>
        /// Gets or sets the DriveId.
        /// </summary>
        public string DriveId { get; set; }

        /// <summary>
        /// Gets or sets the ParentFileId.
        /// </summary>
        public string ParentFileId { get; set; } = "root";

        /// <summary>
        /// Gets or sets the PartInfoList.
        /// </summary>
        public FileUploadPartInfo[] PartInfoList { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public FileType Type { get; set; } = FileType.File;

        /// <summary>
        /// Gets or sets the CheckNameMode.
        /// </summary>
        public CheckNameModeType CheckNameMode { get; set; }

        /// <summary>
        /// Gets or sets the ContentHash.
        /// </summary>
        public string ContentHash { get; set; }

        /// <summary>
        /// Gets or sets the ContentHashName.
        /// </summary>
        public string ContentHashName { get; set; }

        /// <summary>
        /// Gets or sets the ProofCode.
        /// </summary>
        public string ProofCode { get; set; }

        /// <summary>
        /// Gets or sets the ProffVersion.
        /// </summary>
        public string ProffVersion { get; set; }
    }
}
