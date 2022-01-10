// -----------------------------------------------------------------------
// <copyright file="AliyunDriveConfig.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using NCloud.FileProviders.Abstractions;

    /// <summary>
    /// Defines the <see cref="AliyunDriveConfig" />.
    /// </summary>
    public class AliyunDriveConfig : BaseProviderConfig
    {
        /// <summary>
        /// Gets the Type.
        /// </summary>
        public override string Type => AliyunDriveFileProvider.Type;

        /// <summary>
        /// Gets or sets the RefreshToken.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// The GetRefreshToken.
        /// </summary>
        /// <param name="configFolder">The configFolder<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public (string refreshToken, DateTime expiredTime) GetRefreshToken(string configFolder)
        {
            var refreshTokenFile = Path.Combine(configFolder, $"refreshtoken-{this.HashKey()}");
            if (File.Exists(refreshTokenFile))
            {
                var fileInfo = new FileInfo(refreshTokenFile);
                return (File.ReadAllText(refreshTokenFile), fileInfo.LastAccessTimeUtc.AddSeconds(7200));
            }
            else
            {
                File.WriteAllText(refreshTokenFile, RefreshToken);
                return (File.ReadAllText(refreshTokenFile), DateTime.UtcNow);
            }
        }

        /// <summary>
        /// The HashKey.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string HashKey()
        {
            var hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes($"aliyundrive:{RefreshToken}"));
            return BitConverter.ToString(hash).Replace("-", string.Empty); ;
        }

        /// <summary>
        /// The GetRefreshToken.
        /// </summary>
        /// <param name="configFolder">The configFolder<see cref="string"/>.</param>
        /// <param name="refreshToken">The refreshToken<see cref="string"/>.</param>
        public void UpdateRefreshToken(string configFolder, string refreshToken)
        {
            var refreshTokenFile = Path.Combine(configFolder, $"refreshtoken-{this.HashKey()}");
            File.WriteAllText(refreshTokenFile, refreshToken);
        }
    }
}
