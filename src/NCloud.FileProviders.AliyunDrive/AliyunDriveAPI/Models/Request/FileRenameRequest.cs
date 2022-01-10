// -----------------------------------------------------------------------
// <copyright file="FileRenameRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request
{
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;

    /// <summary>
    /// Defines the <see cref="FileRenameRequest" />.
    /// </summary>
    public class FileRenameRequest : FileBaseRequest
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the CheckNameMode.
        /// </summary>
        public CheckNameModeType CheckNameMode { get; set; } = CheckNameModeType.Refuse;
    }
}
