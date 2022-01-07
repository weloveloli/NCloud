// -----------------------------------------------------------------------
// <copyright file="AliyunDriveFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive
{
    using System;
    using AliyunDriveAPI;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.Support;

    /// <summary>
    /// Defines the <see cref="AliyunDriveFileProvider" />.
    /// </summary>
    public class AliyunDriveFileProvider : PrefixNCloudFileProvider<AliyunDriveConfig>
    {
        /// <summary>
        /// Defines the systemConfigProvider.
        /// </summary>
        private readonly ISystemConfigProvider systemConfigProvider;

        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly AliyunDriveApiClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AliyunDriveFileProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="AliyunDriveConfig"/>.</param>
        public AliyunDriveFileProvider(IServiceProvider provider, AliyunDriveConfig config) : base(provider, config)
        {
            this.systemConfigProvider = provider.GetService<ISystemConfigProvider>();
            var refreshToken = config.GetRefreshToken(systemConfigProvider.ConfigFolder);
            this.client = new AliyunDriveApiClient(refreshToken);
        }

        /// <summary>
        /// The GetDirectoryContentsByRelPath.
        /// </summary>
        /// <param name="relPath">The relPath<see cref="string"/>.</param>
        /// <returns>The <see cref="IDirectoryContents"/>.</returns>
        protected override IDirectoryContents GetDirectoryContentsByRelPath(string relPath)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// The GetFileInfoByRelPath.
        /// </summary>
        /// <param name="relPath">The relPath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        protected override IFileInfo GetFileInfoByRelPath(string relPath)
        {
            throw new System.NotImplementedException();
        }
    }
}
