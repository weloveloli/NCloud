﻿// -----------------------------------------------------------------------
// <copyright file="FtpServerOptions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP.Configurations
{
    /// <summary>
    /// Defines the <see cref="FtpServerOptions" />.
    /// </summary>
    /// <summary>
    /// Gets or sets server options.
    /// </summary>
    public class FtpServerOptions
    {
        /// <summary>
        /// Gets or sets the FTP server address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the active FTP data connections should be bound to <see cref="Port"/> - 1.
        /// </summary>
        public bool UseFtpDataPort { get; set; }

        /// <summary>
        /// Gets or sets the PASV options.
        /// </summary>
        public FtpServerPasvOptions Pasv { get; set; } = new FtpServerPasvOptions();

        /// <summary>
        /// Gets or sets the max allows active connections.
        /// </summary>
        /// <remarks>
        /// This will cause connections to be refused if count is exceeded.
        /// 0 (default) means no control over connection count.
        /// </remarks>
        public int? MaxActiveConnections { get; set; }

        /// <summary>
        /// Gets or sets the interval between checks for inactive connections.
        /// </summary>
        public int? ConnectionInactivityCheckInterval { get; set; }
    }
}
