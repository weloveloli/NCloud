// -----------------------------------------------------------------------
// <copyright file="FtpOptionsExtensions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP.Configurations
{
    using System;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="FtpOptionsExtensions" />.
    /// </summary>
    public static class FtpOptionsExtensions
    {
        /// <summary>
        /// Gets the requested or the default port.
        /// </summary>
        /// <param name="options">The FTP options.</param>
        /// <returns>The FTP server port.</returns>
        public static int GetServerPort(this FtpOptions options)
        {
            return options.Server.Port ?? 21;
        }

        /// <summary>
        /// Gets the PASV/EPSV port range.
        /// </summary>
        /// <param name="options">The FTP options.</param>
        /// <returns>The port range.</returns>
        public static (int from, int to)? GetPasvPortRange(this FtpOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.Server.Pasv.Range))
            {
                return null;
            }

            var portRange = options.Server.Pasv.Range!.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            if (portRange.Length != 2)
            {
                throw new ApplicationException("Need exactly two ports for PASV port range");
            }

            var iPorts = portRange.Select(s => Convert.ToInt32(s)).ToArray();

            if (iPorts[1] < iPorts[0])
            {
                throw new ApplicationException("PASV start port must be smaller than end port");
            }

            return (iPorts[0], iPorts[1]);
        }
    }
}
