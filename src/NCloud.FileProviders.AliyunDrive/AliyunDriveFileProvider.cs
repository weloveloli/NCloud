// -----------------------------------------------------------------------
// <copyright file="AliyunDriveFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models;
    using NCloud.FileProviders.Support;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="AliyunDriveFileProvider" />.
    /// </summary>
    [FileProvider(Name = Type, Type = Type)]
    public class AliyunDriveFileProvider : PrefixNCloudFileProvider<AliyunDriveConfig>
    {
        public const string Type = "aliyundrive";
        /// <summary>
        /// Defines the systemConfigProvider.
        /// </summary>
        private readonly ISystemConfigProvider systemConfigProvider;

        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly AliyunDriveClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AliyunDriveFileProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="AliyunDriveConfig"/>.</param>
        public AliyunDriveFileProvider(IServiceProvider provider, AliyunDriveConfig config) : base(provider, config)
        {
            this.systemConfigProvider = provider.GetService<ISystemConfigProvider>();
            this.client = new AliyunDriveClient(config, systemConfigProvider.ConfigFolder, provider.GetService<IMemoryCache>());
        }

        /// <summary>
        /// The GetDirectoryContentsByRelPath.
        /// </summary>
        /// <param name="relPath">The relPath<see cref="string"/>.</param>
        /// <returns>The <see cref="IDirectoryContents"/>.</returns>
        protected override IDirectoryContents GetDirectoryContentsByRelPath(string relPath)
        {
            relPath = relPath.EnsureStartsWith('/');
            var items = client.GetFileItemsByPath(relPath);
            if (items != null && items.Any())
            {
                return new EnumerableDirectoryContents(items.Select(e => this.ToFileInfo(e)));
            }
            return null;
        }

        /// <summary>
        /// The ToFileInfo.
        /// </summary>
        /// <param name="e">The e<see cref="FileItem"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        private IFileInfo ToFileInfo(FileItem e)
        {
            return new AliyunDriveFileInfo(e, client);
        }

        /// <summary>
        /// The GetFileInfoByRelPath.
        /// </summary>
        /// <param name="relPath">The relPath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        protected override IFileInfo GetFileInfoByRelPath(string relPath)
        {
            relPath = relPath.EnsureStartsWith('/');
            var item = client.GetFileItemByPath(relPath);
            if (item == null)
            {
                return null;
            }
            else
            {
                return ToFileInfo(item);
            }
        }
    }
}
