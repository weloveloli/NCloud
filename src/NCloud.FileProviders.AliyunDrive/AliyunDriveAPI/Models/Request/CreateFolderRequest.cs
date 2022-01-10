// -----------------------------------------------------------------------
// <copyright file="CreateFolderRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request
{
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;

    /// <summary>
    /// Defines the <see cref="CreateFolderRequest" />.
    /// </summary>
    public class CreateFolderRequest
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
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the CheckNameMode.
        /// </summary>
        public CheckNameModeType CheckNameMode { get; set; } = CheckNameModeType.Refuse;

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public FileType Type { get; set; } = FileType.Folder;
    }
}
