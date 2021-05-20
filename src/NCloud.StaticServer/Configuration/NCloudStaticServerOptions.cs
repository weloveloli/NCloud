﻿// -----------------------------------------------------------------------
// <copyright file="NCloudStaticServerOptions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.StaticServer.Configuration
{
    using NCloud.EndPoints.FTP.Configurations;

    /// <summary>
    /// Defines the <see cref="NCloudStaticServerOptions" />.
    /// </summary>
    public class NCloudStaticServerOptions
    {
        /// <summary>
        /// Gets or sets the Ftp.
        /// </summary>
        public FtpOptions Ftp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ftp endpoint is Enable.
        /// </summary>
        public bool FtpEnable { get; set; }
    }
}
