// -----------------------------------------------------------------------
// <copyright file="FileShareRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="FileShareRequest" />.
    /// </summary>
    public class FileShareRequest
    {
        /// <summary>
        /// Gets or sets the DriveId.
        /// </summary>
        public string DriveId { get; set; }

        /// <summary>
        /// Gets or sets the FileIdList.
        /// </summary>
        public List<string> FileIdList { get; set; }

        /// <summary>
        /// Gets or sets the Expiration.
        /// </summary>
        public DateTime? Expiration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SyncToHomepage.
        /// </summary>
        public bool SyncToHomepage { get; set; }

        /// <summary>
        /// Gets or sets the SharePwd.
        /// </summary>
        public string SharePwd { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileShareRequest"/> class.
        /// </summary>
        public FileShareRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileShareRequest"/> class.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <param name="expiration">The expiration<see cref="TimeSpan"/>.</param>
        public FileShareRequest(string driveId, string fileId, TimeSpan expiration)
            : this(driveId, fileId, expiration, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileShareRequest"/> class.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <param name="sharePwd">The sharePwd<see cref="string"/>.</param>
        public FileShareRequest(string driveId, string fileId, string sharePwd)
            : this(driveId, fileId, null, sharePwd)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileShareRequest"/> class.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <param name="expiration">The expiration<see cref="TimeSpan?"/>.</param>
        /// <param name="sharePwd">The sharePwd<see cref="string"/>.</param>
        public FileShareRequest(string driveId, string fileId, TimeSpan? expiration = null, string sharePwd = null)
           : this(driveId, new List<string>() { fileId }, expiration, sharePwd)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileShareRequest"/> class.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileIdList">The fileIdList<see cref="List{string}"/>.</param>
        /// <param name="expiration">The expiration<see cref="TimeSpan"/>.</param>
        public FileShareRequest(string driveId, List<string> fileIdList, TimeSpan expiration)
            : this(driveId, fileIdList, expiration, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileShareRequest"/> class.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileIdList">The fileIdList<see cref="List{string}"/>.</param>
        /// <param name="sharePwd">The sharePwd<see cref="string"/>.</param>
        public FileShareRequest(string driveId, List<string> fileIdList, string sharePwd)
            : this(driveId, fileIdList, null, sharePwd)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileShareRequest"/> class.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileIdList">The fileIdList<see cref="List{string}"/>.</param>
        /// <param name="expiration">The expiration<see cref="TimeSpan?"/>.</param>
        /// <param name="sharepwd">The sharepwd<see cref="string"/>.</param>
        /// <param name="syncToHomepage">The syncToHomepage<see cref="bool"/>.</param>
        public FileShareRequest(string driveId
                                , List<string> fileIdList
                                , TimeSpan? expiration = null
                                , string sharepwd = null
                                , bool syncToHomepage = false)
        {
            DriveId = driveId;
            FileIdList = fileIdList;
            if (expiration.HasValue)
            {
                Expiration = DateTime.Now.AddMilliseconds(expiration.Value.TotalMilliseconds);
            }
            SharePwd = sharepwd;
            SyncToHomepage = syncToHomepage;
        }
    }
}
