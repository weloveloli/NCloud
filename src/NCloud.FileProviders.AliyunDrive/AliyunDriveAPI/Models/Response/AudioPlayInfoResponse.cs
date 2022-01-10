// -----------------------------------------------------------------------
// <copyright file="AudioPlayInfoResponse.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response
{
    /// <summary>
    /// Defines the <see cref="AudioPlayInfoResponse" />.
    /// </summary>
    public class AudioPlayInfoResponse
    {
        /// <summary>
        /// Gets or sets the TemplateList.
        /// </summary>
        public AudioPlayInfo[] TemplateList { get; set; }
    }
}
