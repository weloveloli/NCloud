// -----------------------------------------------------------------------
// <copyright file="AliyunDriveClient.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response;

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
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<AliyunDriveClient> logger;

        /// <summary>
        /// Defines the configFolder.
        /// </summary>
        private readonly string configFolder;

        /// <summary>
        /// Defines the cache.
        /// </summary>
        private readonly IMemoryCache cache;

        /// <summary>
        /// Defines the defaultDriveId.
        /// </summary>
        private readonly string defaultDriveId;

        /// <summary>
        /// Defines the rootFileId.
        /// </summary>
        private readonly string rootFileId;

        /// <summary>
        /// Defines the idCacheOption.
        /// </summary>
        private readonly MemoryCacheEntryOptions idCacheOption;

        /// <summary>
        /// Defines the itemCacheOption.
        /// </summary>
        private readonly MemoryCacheEntryOptions itemCacheOption;

        /// <summary>
        /// Defines the itemCacheOption.
        /// </summary>
        private readonly MemoryCacheEntryOptions downloadUrlCacheOption;
        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly AliyunDriveApiClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AliyunDriveClient"/> class.
        /// </summary>
        /// <param name="config">The config<see cref="AliyunDriveConfig"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{AliyunDriveClient}"/>.</param>
        /// <param name="configFolder">The configFolder<see cref="string"/>.</param>
        /// <param name="cache">The cache<see cref="IMemoryCache"/>.</param>
        public AliyunDriveClient(AliyunDriveConfig config, ILogger<AliyunDriveClient> logger, string configFolder, IMemoryCache cache)
        {
            this.config = config;
            this.logger = logger;
            this.configFolder = configFolder;
            this.cache = cache;
            var token = config.GetRefreshToken(configFolder);
            this.client = new AliyunDriveApiClient(token.refreshToken, token.expiredTime, (res) =>
            {
                config.UpdateRefreshToken(configFolder, res.RefreshToken);
            });
            var res = this.client.RefreshTokenAsync().Result;
            defaultDriveId = res.DefaultDriveId;
            rootFileId = "root";
            this.idCacheOption = new MemoryCacheEntryOptions()
                            .SetSize(1)
                            .SetPriority(CacheItemPriority.High)
                            // Remove from cache after this time, regardless of sliding expiration
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

            this.itemCacheOption = new MemoryCacheEntryOptions()
                .SetSize(1)
                .SetPriority(CacheItemPriority.Normal)
                // Remove from cache after this time, regardless of sliding expiration
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

            this.downloadUrlCacheOption = new MemoryCacheEntryOptions()
                .SetSize(1)
                .SetPriority(CacheItemPriority.Normal)
                // Remove from cache after this time, regardless of sliding expiration
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(20));
        }

        /// <summary>
        /// The GetFileItemByPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="FileItem"/>.</returns>
        public FileItem GetFileItemByPath(string path = "/")
        {
            logger.LogDebug("GetFileItemByPath :{path}", path);
            var fileId = GetFileIdByPath(path);
            if (string.IsNullOrEmpty(fileId))
            {
                return null;
            }

            var item = cache.Get<FileItem>("GetFileItemByPath:" + fileId);
            if (item != null)
            {
                logger.LogDebug("GetFileItemByPath with cache :{path}", path);
                return item;
            }
            logger.LogDebug("GetFileItemByPath miss cache :{path}", path);
            if (client.IsTokenExpire())
            {
                client.RefreshTokenAsync().Wait();
            }

            item = this.client.FileGetAsync(defaultDriveId, fileId).Result;
            if (item != null)
            {
                cache.Set("GetFileItemByPath:" + fileId, item, itemCacheOption);
            }
            return item;
        }

        /// <summary>
        /// The GetFileIdByPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetFileIdByPath(string path)
        {
            logger.LogDebug("GetFileIdByPath :{path}", path);
            if (path == "/")
            {
                return rootFileId;
            }
            if (path.EndsWith("/"))
            {
                path = path.TrimEnd('/');
            }
            var fileId = cache.Get<string>("FileIdByPath:" + path);
            if (!string.IsNullOrEmpty(fileId))
            {
                logger.LogDebug("GetFileIdByPath with cache :{path}", path);
                return fileId;
            }
            logger.LogDebug("GetFileIdByPath without cache :{path}", path);
            var index = path.LastIndexOf("/");
            var parentPath = path.Substring(0, index);
            var name = path.Substring(index + 1);
            if (client.IsTokenExpire())
            {
                client.RefreshTokenAsync().Wait();
            }
            var items = GetFileItemsByPath(parentPath);
            var item = items.Where(e => e.Name == name).FirstOrDefault();
            if (item != null)
            {
                logger.LogDebug("GetFileIdByPath set cache :{path}", path);
                cache.Set("FileIdByPath:" + path, item.FileId, idCacheOption);
            }
            return item?.FileId ?? string.Empty;
        }

        /// <summary>
        /// The GetFileItemsByPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="FileItem[]"/>.</returns>
        public IEnumerable<FileItem> GetFileItemsByPath(string path = "/")
        {
            logger.LogDebug("GetFileItemsByPath :{path}", path);
            if (string.IsNullOrEmpty(path))
            {
                path = "/";
            }
            var fileId = GetFileIdByPath(path);
            if (string.IsNullOrEmpty(fileId))
            {
                return Enumerable.Empty<FileItem>();
            }
            var listRes = cache.Get<FileListResponse>("GetFileItemsByPath:" + fileId);
            if (listRes != null)
            {
                logger.LogDebug("GetFileItemsByPath with cache:{path}", path);
                return listRes.Items;
            }
            logger.LogDebug("GetFileItemsByPath without cache:{path}", path);
            if (client.IsTokenExpire())
            {
                client.RefreshTokenAsync().Wait();
            }
            listRes = this.client.FileListAsync(new FileListRequest
            {
                DriveId = defaultDriveId,
                ParentFileId = fileId
            }).Result;
            if (listRes != null)
            {
                logger.LogDebug("GetFileItemsByPath set cache:{path}", path);
                cache.Set("GetFileItemsByPath:" + fileId, listRes, itemCacheOption);
            }
            return listRes?.Items ?? Enumerable.Empty<FileItem>();
        }

        /// <summary>
        /// The GetDownloadLinkAsync.
        /// </summary>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public async Task<DownloadUrlResponse> GetDownloadLinkAsync(string fileId)
        {
            logger.LogDebug("GetDownloadLinkAsync :{fileId}", fileId);
            var res = cache.Get<DownloadUrlResponse>("GetDownloadLinkAsync:" + fileId);
            if (res == null)
            {
                if (client.IsTokenExpire())
                {
                    client.RefreshTokenAsync().Wait();
                }
                res = await this.client.GetDownloadUrlAsync(this.defaultDriveId, fileId);
                if (res != null)
                {
                    cache.Set("GetDownloadLinkAsync:" + fileId, res, downloadUrlCacheOption);
                }
            }
            return res;
        }

        public void Refresh()
        {
            this.client.RefreshTokenAsync().Wait();
        }
    }
}
