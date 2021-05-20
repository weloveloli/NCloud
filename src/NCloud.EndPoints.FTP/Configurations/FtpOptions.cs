// -----------------------------------------------------------------------
// <copyright file="FtpOptions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP.Configurations
{
    /// <summary>
    /// Defines the <see cref="FtpOptions" />.
    /// </summary>
    public class FtpOptions
    {
        /// <summary>
        /// Gets or sets authentication providers to use.
        /// </summary>
        public MembershipProviderType Authentication { get; set; } = MembershipProviderType.Default;

        /// <summary>
        /// Gets or sets the FTP server options.
        /// </summary>
        public FtpServerOptions Server { get; set; } = new FtpServerOptions();
    }
}
