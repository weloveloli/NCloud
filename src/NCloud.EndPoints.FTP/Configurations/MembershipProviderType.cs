// -----------------------------------------------------------------------
// <copyright file="MembershipProviderType.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP.Configurations
{
    using System;

    /// <summary>
    /// Defines the MembershipProviderType.
    /// </summary>
    [Flags]
    public enum MembershipProviderType
    {
        /// <summary>
        /// Use the default membership provider (<see cref="Anonymous"/>).
        /// </summary>
        Default = 0,
        /// <summary>
        /// Use the custom (example) membership provider.
        /// </summary>
        Custom = 1,
        /// <summary>
        /// Use the membership provider for anonymous users.
        /// </summary>
        Anonymous = 2,
        /// <summary>
        /// Use the PAM membership provider.
        /// </summary>
        PAM = 4,
    }
}
