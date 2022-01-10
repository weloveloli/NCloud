// -----------------------------------------------------------------------
// <copyright file="FileBaseRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request
{
    /// <summary>
    /// Defines the <see cref="FileBaseRequest" />.
    /// </summary>
    public class FileBaseRequest
    {
        /// <summary>
        /// Gets or sets the DriveId.
        /// </summary>
        public string DriveId { get; set; }

        /// <summary>
        /// Gets or sets the FileId.
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileBaseRequest"/> class.
        /// </summary>
        public FileBaseRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileBaseRequest"/> class.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        public FileBaseRequest(string driveId, string fileId)
        {
            DriveId = driveId;
            FileId = fileId;
        }
    }
}
