// -----------------------------------------------------------------------
// <copyright file="FileGetResponse.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response
{
    /// <summary>
    /// Defines the <see cref="FileGetResponse" />.
    /// </summary>
    public class FileGetResponse : FileItem
    {
        /// <summary>
        /// Gets or sets the ExFieldsInfo.
        /// </summary>
        public ExFieldsInfo ExFieldsInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Trashed.
        /// </summary>
        public bool Trashed { get; set; }
    }
}
