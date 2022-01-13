// -----------------------------------------------------------------------
// <copyright file="AliyunDriveFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Logging;
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
        /// <summary>
        /// Defines the Type.
        /// </summary>
        public const string Type = "aliyundrive";

        /// <summary>
        /// Defines the systemConfigProvider.
        /// </summary>
        private readonly ISystemConfigProvider systemConfigProvider;

        /// <summary>
        /// Defines the httpClient.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly AliyunDriveClient client;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<AliyunDriveFileProvider> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AliyunDriveFileProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="AliyunDriveConfig"/>.</param>
        public AliyunDriveFileProvider(IServiceProvider provider, AliyunDriveConfig config) : base(provider, config)
        {
            this.systemConfigProvider = provider.GetService<ISystemConfigProvider>();
            var clientLogger = provider.GetService<ILogger<AliyunDriveClient>>();
            this.httpClient = (HttpClient)provider.GetService(typeof(HttpClient)) ?? new HttpClient();
            this.client = new AliyunDriveClient(config, clientLogger, systemConfigProvider.ConfigFolder, provider.GetService<IMemoryCache>());
            this.logger = provider.GetService<ILogger<AliyunDriveFileProvider>>();
        }

        /// <summary>
        /// The GetDirectoryContentsByRelPath.
        /// </summary>
        /// <param name="relPath">The relPath<see cref="string"/>.</param>
        /// <returns>The <see cref="IDirectoryContents"/>.</returns>
        protected override IDirectoryContents GetDirectoryContentsByRelPath(string relPath)
        {
            relPath = relPath.EnsureStartsWith('/');
            var stopwatch = Stopwatch.StartNew();
            this.logger.LogDebug("GetDirectoryContentsByRelPath {relPath}", relPath);
            var items = client.GetFileItemsByPath(relPath);
            stopwatch.Stop();
            this.logger.LogDebug("GetDirectoryContentsByRelPath {relPath}, ElapsedMilliseconds {ElapsedMilliseconds}", relPath, stopwatch.ElapsedMilliseconds);
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
            return new AliyunDriveFileInfo(e, client, httpClient);
        }

        /// <summary>
        /// The GetFileInfoByRelPath.
        /// </summary>
        /// <param name="relPath">The relPath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        protected override IFileInfo GetFileInfoByRelPath(string relPath)
        {
            relPath = relPath.EnsureStartsWith('/');
            var stopwatch = Stopwatch.StartNew();
            this.logger.LogDebug("GetFileInfoByRelPath {relPath}", relPath);
            var item = client.GetFileItemByPath(relPath);
            stopwatch.Stop();
            this.logger.LogDebug("GetFileInfoByRelPath {relPath}, ElapsedMilliseconds {ElapsedMilliseconds}", relPath, stopwatch.ElapsedMilliseconds);
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
