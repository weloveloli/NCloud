// -----------------------------------------------------------------------
// <copyright file="AliyunDriveClient.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive
{
    using AliyunDriveAPI;

    /// <summary>
    /// Defines the <see cref="AliyunDriveClient" />.
    /// </summary>
    public class AliyunDriveClient
    {
        /// <summary>
        /// Defines the config.
        /// </summary>
        private readonly AliyunDriveConfig config;

        /// <summary>
        /// Defines the configFolder.
        /// </summary>
        private readonly string configFolder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AliyunDriveClient"/> class.
        /// </summary>
        /// <param name="config">The config<see cref="AliyunDriveConfig"/>.</param>
        /// <param name="configFolder">The configFolder<see cref="string"/>.</param>
        public AliyunDriveClient(AliyunDriveConfig config, string configFolder)
        {
            this.config = config;
            this.configFolder = configFolder;
            var refreshToken = config.GetRefreshToken(configFolder);
            var client = new AliyunDriveApiClient(refreshToken);
        }
    }
}
