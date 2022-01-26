// -----------------------------------------------------------------------
// <copyright file="WebDAVConfig.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.WebDAV.Configurations
{
    /// <summary>
    /// Defines the <see cref="WebDAVConfig" />.
    /// </summary>
    public class WebDAVConfig
    {
        /// <summary>
        /// Gets or sets the Protocol.
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the Ip.
        /// </summary>
        public string Ip { get; set; } = "*";

        /// <summary>
        /// Gets or sets the Port.
        /// </summary>
        public string Port { get; set; } = "11111";

        /// <summary>
        /// Gets or sets a value indicating whether Authentication.
        /// </summary>
        public bool Authentication { get; set; } = true;

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        public string UserName { get; set; } = "admin";

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; } = "admin";
    }
}
