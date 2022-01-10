// -----------------------------------------------------------------------
// <copyright file="RecyclebinListRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request
{
    /// <summary>
    /// Defines the <see cref="RecyclebinListRequest" />.
    /// </summary>
    public class RecyclebinListRequest : FileListRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecyclebinListRequest"/> class.
        /// </summary>
        public RecyclebinListRequest()
        {
            ParentFileId = null;
            Limit = null;
            All = null;
            UrlExpireSec = null;
            Fields = null;
        }
    }
}
