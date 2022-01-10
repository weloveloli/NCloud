// -----------------------------------------------------------------------
// <copyright file="RefreshTokenResponse.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response
{
    using System;

    /// <summary>
    /// Defines the <see cref="RefreshTokenResponse" />.
    /// </summary>
    public class RefreshTokenResponse
    {
        /// <summary>
        /// Gets or sets the DefaultSboxDriveId.
        /// </summary>
        public string DefaultSboxDriveId { get; set; }

        /// <summary>
        /// Gets or sets the Role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the DeviceId.
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether NeedLink.
        /// </summary>
        public bool NeedLink { get; set; }

        /// <summary>
        /// Gets or sets the ExpireTime.
        /// </summary>
        public DateTime? ExpireTime { get; set; }

        /// <summary>
        /// Gets or sets the PinSetup.
        /// </summary>
        public bool? PinSetup { get; set; }

        /// <summary>
        /// Gets or sets the NeedRpVerify.
        /// </summary>
        public bool? NeedRpVerify { get; set; }

        /// <summary>
        /// Gets or sets the Avatar.
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Gets or sets the TokenType.
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the AccessToken.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the DefaultDriveId.
        /// </summary>
        public string DefaultDriveId { get; set; }

        /// <summary>
        /// Gets or sets the DomainId.
        /// </summary>
        public string DomainId { get; set; }

        /// <summary>
        /// Gets or sets the RefreshToken.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the IsFirstLogin.
        /// </summary>
        public bool? IsFirstLogin { get; set; }

        /// <summary>
        /// Gets or sets the UserId.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the NickName.
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// Gets or sets the State.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the ExpiresIn.
        /// </summary>
        public int? ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        public string Status { get; set; }
    }
}
