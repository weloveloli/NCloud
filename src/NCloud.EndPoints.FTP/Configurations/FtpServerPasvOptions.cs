// -----------------------------------------------------------------------
// <copyright file="FtpServerPasvOptions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP.Configurations
{
    /// <summary>
    /// The options for PASV/EPSV.
    /// </summary>
    public class FtpServerPasvOptions
    {
        /// <summary>
        /// Gets or sets the port range.
        /// </summary>
        public string Range { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether promiscuous PASV is allowed.
        /// </summary>
        public bool Promiscuous { get; set; }
    }
}
