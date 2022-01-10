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
    using Microsoft.Extensions.Caching.Memory;
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
        private readonly MemoryCacheEntryOptions idCacheOption;
        private readonly MemoryCacheEntryOptions itemCacheOption;

        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly AliyunDriveApiClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AliyunDriveClient"/> class.
        /// </summary>
        /// <param name="config">The config<see cref="AliyunDriveConfig"/>.</param>
        /// <param name="configFolder">The configFolder<see cref="string"/>.</param>
        /// <param name="cache">The cache<see cref="IMemoryCache"/>.</param>
        public AliyunDriveClient(AliyunDriveConfig config, string configFolder, IMemoryCache cache)
        {
            this.config = config;
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
                            .SetSize(128)
                            .SetPriority(CacheItemPriority.High)
                            // Remove from cache after this time, regardless of sliding expiration
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

            this.itemCacheOption = new MemoryCacheEntryOptions()
                .SetSize(128)
                .SetPriority(CacheItemPriority.High)
                // Remove from cache after this time, regardless of sliding expiration
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        }

        /// <summary>
        /// The GetFileItemByPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="FileItem"/>.</returns>
        public FileItem GetFileItemByPath(string path = "/")
        {
            var fileId = GetFileIdByPath(path);
            if (string.IsNullOrEmpty(fileId))
            {
                return null;
            }

            var item = cache.Get<FileItem>("GetFileItemByPath:" + fileId);
            if (item != null)
            {
                return item;
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
                return fileId;
            }
            var index = path.LastIndexOf("/");
            var parentPath = path.Substring(0, index);
            var name = path.Substring(index + 1);
            var items = GetFileItemsByPath(parentPath);
            var item = items.Where(e => e.Name == name).FirstOrDefault();
            if (item != null)
            {
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
                return listRes.Items;
            }
            listRes = this.client.FileListAsync(new FileListRequest
            {
                DriveId = defaultDriveId,
                ParentFileId = fileId
            }).Result;
            if (listRes != null)
            {
                cache.Set("GetFileItemsByPath:" + fileId, listRes, itemCacheOption);
            }
            return listRes?.Items ?? Enumerable.Empty<FileItem>();
        }
    }
}
