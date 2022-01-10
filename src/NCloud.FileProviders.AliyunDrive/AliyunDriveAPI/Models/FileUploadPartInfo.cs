// -----------------------------------------------------------------------
// <copyright file="FileUploadPartInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models
{
    /// <summary>
    /// Defines the <see cref="FileUploadPartInfo" />.
    /// </summary>
    public class FileUploadPartInfo
    {
        /// <summary>
        /// Gets or sets the PartNumber.
        /// </summary>
        public int PartNumber { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploadPartInfo"/> class.
        /// </summary>
        public FileUploadPartInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploadPartInfo"/> class.
        /// </summary>
        /// <param name="partNumber">The partNumber<see cref="int"/>.</param>
        public FileUploadPartInfo(int partNumber)
        {
            PartNumber = partNumber;
        }
    }

    /// <summary>
    /// Defines the <see cref="FileUploadPartInfoWithUrl" />.
    /// </summary>
    public class FileUploadPartInfoWithUrl : FileUploadPartInfo
    {
        /// <summary>
        /// Gets or sets the UploadUrl.
        /// </summary>
        public string UploadUrl { get; set; }

        /// <summary>
        /// Gets or sets the InternalUploadUrl.
        /// </summary>
        public string InternalUploadUrl { get; set; }

        /// <summary>
        /// Gets or sets the ContentType.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploadPartInfoWithUrl"/> class.
        /// </summary>
        public FileUploadPartInfoWithUrl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploadPartInfoWithUrl"/> class.
        /// </summary>
        /// <param name="partNumber">The partNumber<see cref="int"/>.</param>
        public FileUploadPartInfoWithUrl(int partNumber) : base(partNumber)
        {
        }
    }
}
