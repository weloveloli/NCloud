// -----------------------------------------------------------------------
// <copyright file="NCloudStore.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.WebDAV
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NCloud.EndPoints.WebDAV.Models;
    using NCloud.FileProviders.Abstractions;
    using NCloud.Utils;
    using NWebDav.Server.Helpers;
    using NWebDav.Server.Http;
    using NWebDav.Server.Locking;
    using NWebDav.Server.Stores;

    /// <summary>
    /// Defines the <see cref="NCloudStore" />.
    /// </summary>
    public class NCloudStore : IStore
    {
        /// <summary>
        /// Defines the iNCloudFileProvider.
        /// </summary>
        private readonly INCloudFileProvider iNCloudFileProvider;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<NCloudStore> logger;

        /// <summary>
        /// Gets the LockingManager.
        /// </summary>
        public ILockingManager LockingManager { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudStore"/> class.
        /// </summary>
        /// <param name="iNCloudFileProvider">The iNCloudFileProvider<see cref="INCloudFileProvider"/>.</param>
        /// <param name="lockingManager">The lockingManager<see cref="ILockingManager"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{NCloudStore}"/>.</param>
        public NCloudStore(INCloudFileProvider iNCloudFileProvider, ILogger<NCloudStore> logger, ILockingManager lockingManager = null)
        {
            this.iNCloudFileProvider = iNCloudFileProvider;
            this.LockingManager = lockingManager ?? new InMemoryLockingManager();
            this.logger = logger;
        }

        /// <summary>
        /// The GetCollectionAsync.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IStoreCollection}"/>.</returns>
        public Task<IStoreCollection> GetCollectionAsync(Uri uri, IHttpContext httpContext)
        {
            this.logger.LogDebug("GetCollectionAsync {uri}", uri);
            // Determine the path from the uri
            var path = GetPathFromUri(uri);
            var content = iNCloudFileProvider.GetDirectoryContents(path);
            var name = path.Substring(path.LastIndexOf('/') + 1);
            return Task.FromResult<IStoreCollection>(new NCloudStoreCollection(LockingManager, path, content, name, this.iNCloudFileProvider));
        }

        /// <summary>
        /// The GetItemAsync.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IStoreItem}"/>.</returns>
        public Task<IStoreItem> GetItemAsync(Uri uri, IHttpContext httpContext)
        {
            this.logger.LogDebug("GetItemAsync {uri}", uri);
            // Determine the path from the uri
            var path = GetPathFromUri(uri);
            var fileInfo = this.iNCloudFileProvider.GetFileInfo(path);
            if (fileInfo.Exists && !fileInfo.IsDirectory)
            {
                return Task.FromResult<IStoreItem>(new NCloudItemStoreItem(fileInfo, this.LockingManager));
            }
            else if (fileInfo.Exists && fileInfo.IsDirectory)
            {
                var content = iNCloudFileProvider.GetDirectoryContents(path);
                return Task.FromResult<IStoreItem>(new NCloudStoreCollection(LockingManager, path, content, fileInfo.Name, this.iNCloudFileProvider));
            }
            else
            {
                return Task.FromResult<IStoreItem>(null);
            }
        }

        /// <summary>
        /// The GetPathFromUri.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string GetPathFromUri(Uri uri)
        {
            // Determine the path
            var requestedPath = UriHelper.GetDecodedPath(uri).Substring(1);

            // Return the combined path
            return requestedPath.TrimEnd('/').EnsureStartsWith('/');
        }
    }
}
