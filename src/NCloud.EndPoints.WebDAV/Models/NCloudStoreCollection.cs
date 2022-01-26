// -----------------------------------------------------------------------
// <copyright file="NCloudStoreCollection.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.WebDAV.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
    using NCloud.Utils;
    using NWebDav.Server;
    using NWebDav.Server.Helpers;
    using NWebDav.Server.Http;
    using NWebDav.Server.Locking;
    using NWebDav.Server.Logging;
    using NWebDav.Server.Props;
    using NWebDav.Server.Stores;

    /// <summary>
    /// Defines the <see cref="NCloudStoreCollection" />.
    /// </summary>
    internal class NCloudStoreCollection : IStoreCollection
    {
        /// <summary>
        /// Defines the s_log.
        /// </summary>
        private static readonly ILogger s_log = LoggerFactory.CreateLogger(typeof(NCloudStoreCollection));

        /// <summary>
        /// Defines the s_xDavCollection.
        /// </summary>
        private static readonly XElement s_xDavCollection = new XElement(WebDavNamespaces.DavNs + "collection");

        /// <summary>
        /// Defines the fullPath.
        /// </summary>
        private readonly string fullPath;

        /// <summary>
        /// Defines the contents.
        /// </summary>
        private readonly IFileInfo fileInfo;

        /// <summary>
        /// Defines the iNCloudFileProvider.
        /// </summary>
        private readonly INCloudFileProvider iNCloudFileProvider;

        /// <summary>
        /// Gets the InfiniteDepthMode.
        /// </summary>
        public InfiniteDepthMode InfiniteDepthMode => InfiniteDepthMode.Assume0;

        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudStoreCollection"/> class.
        /// </summary>
        /// <param name="lockingManager">The lockingManager<see cref="ILockingManager"/>.</param>
        /// <param name="fullPath">The fullPath<see cref="string"/>.</param>
        /// <param name="contents">The contents<see cref="IDirectoryContents"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="iNCloudFileProvider">The iNCloudFileProvider<see cref="INCloudFileProvider"/>.</param>
        public NCloudStoreCollection(ILockingManager lockingManager, string fullPath, IFileInfo fileInfo, string name, INCloudFileProvider iNCloudFileProvider)
        {
            this.LockingManager = lockingManager;
            this.fullPath = fullPath;
            this.fileInfo = fileInfo;
            this.iNCloudFileProvider = iNCloudFileProvider;
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the UniqueKey.
        /// </summary>
        public string UniqueKey => this.fullPath;

        /// <summary>
        /// Gets the DefaultPropertyManager.
        /// </summary>
        public static PropertyManager<NCloudStoreCollection> DefaultPropertyManager { get; } = new PropertyManager<NCloudStoreCollection>(new DavProperty<NCloudStoreCollection>[]
        {
            // RFC-2518 properties
            new DavCreationDate<NCloudStoreCollection>
            {
                Getter = (context, collection) => DateTime.UtcNow,
                Setter = (context, collection, value) =>
                {
                    return DavStatusCode.Ok;
                }
            },
            new DavGetContentType<NCloudStoreCollection>
            {
                Getter = (context, item) => "httpd/unix-directory"
            },
            new DavGetLastModified<NCloudStoreCollection>
            {
                Getter = (context, collection) => DateTime.UtcNow,
                Setter = (context, collection, value) =>
                {
                    return DavStatusCode.Ok;
                }
            },
            // Default locking property handling via the LockingManager
            new DavLockDiscoveryDefault<NCloudStoreCollection>(),
            new DavGetResourceType<NCloudStoreCollection>
            {
                Getter = (context, collection) => new []{s_xDavCollection}
            },
            new DavSupportedLockDefault<NCloudStoreCollection>(),
            new DavExtCollectionQuotaAvailableBytes<NCloudStoreCollection>
            {
                IsExpensive = true,
                Getter = (context, item) => 1L
            },
            new DavExtCollectionQuotaUsedBytes<NCloudStoreCollection>
            {
                IsExpensive = true,
                Getter = (context, item) => 1L
            },
        });

        /// <summary>
        /// Gets or sets the LockingManager.
        /// </summary>
        public ILockingManager LockingManager { get; set; }

        /// <summary>
        /// Gets the PropertyManager.
        /// </summary>
        public IPropertyManager PropertyManager => DefaultPropertyManager;

        /// <summary>
        /// The CopyAsync.
        /// </summary>
        /// <param name="destination">The destination<see cref="IStoreCollection"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{StoreItemResult}"/>.</returns>
        public Task<StoreItemResult> CopyAsync(IStoreCollection destination, string name, bool overwrite, IHttpContext httpContext)
        {
            return Task.FromResult(new StoreItemResult(DavStatusCode.PreconditionFailed));
        }

        /// <summary>
        /// The CreateCollectionAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{StoreCollectionResult}"/>.</returns>
        public Task<StoreCollectionResult> CreateCollectionAsync(string name, bool overwrite, IHttpContext httpContext)
        {
            return Task.FromResult(new StoreCollectionResult(DavStatusCode.PreconditionFailed));
        }

        /// <summary>
        /// The CreateItemAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{StoreItemResult}"/>.</returns>
        public Task<StoreItemResult> CreateItemAsync(string name, bool overwrite, IHttpContext httpContext)
        {
            return Task.FromResult(new StoreItemResult(DavStatusCode.PreconditionFailed));
        }

        /// <summary>
        /// The DeleteItemAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{DavStatusCode}"/>.</returns>
        public Task<DavStatusCode> DeleteItemAsync(string name, IHttpContext httpContext)
        {
            return Task.FromResult(DavStatusCode.PreconditionFailed);
        }

        /// <summary>
        /// The GetItemAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IStoreItem}"/>.</returns>
        public Task<IStoreItem> GetItemAsync(string name, IHttpContext httpContext)
        {
            // Determine the full path
            var fullPath = Path.Combine(this.fullPath, name);
            var item = this.iNCloudFileProvider.GetFileInfo(fullPath);
            if (!item.IsDirectory && item.Exists)
            {
                return Task.FromResult<IStoreItem>(new NCloudItemStoreItem(item, this.LockingManager));
            }
            else if(item.IsDirectory && item.Exists)
            {
                return Task.FromResult<IStoreItem>(new NCloudStoreCollection(this.LockingManager, fullPath, item, name, this.iNCloudFileProvider));
            }
            // Item not found
            return Task.FromResult<IStoreItem>(null);
        }

        /// <summary>
        /// The GetItemsAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{IStoreItem}}"/>.</returns>
        public Task<IEnumerable<IStoreItem>> GetItemsAsync(IHttpContext httpContext)
        {
            IEnumerable<IStoreItem> GetItemsInternal()
            {
                var contents = iNCloudFileProvider.GetDirectoryContents(fullPath);
                // Add all directories
                foreach (var item in contents)
                {
                    if(item.Exists && item.IsDirectory)
                    {
                        yield return new NCloudStoreCollection(LockingManager, fullPath.EnsureEndsWith('/') + item.Name, item, item.Name, iNCloudFileProvider);
                    }
                    if (item.Exists && !item.IsDirectory)
                    {
                        yield return new NCloudItemStoreItem(item,LockingManager);
                    }
                }

            }

            return Task.FromResult(GetItemsInternal());
        }

        /// <summary>
        /// The GetReadableStreamAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public Task<Stream> GetReadableStreamAsync(IHttpContext httpContext)
        {
            return Task.FromResult((Stream)null);
        }

        /// <summary>
        /// The MoveItemAsync.
        /// </summary>
        /// <param name="sourceName">The sourceName<see cref="string"/>.</param>
        /// <param name="destination">The destination<see cref="IStoreCollection"/>.</param>
        /// <param name="destinationName">The destinationName<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{StoreItemResult}"/>.</returns>
        public Task<StoreItemResult> MoveItemAsync(string sourceName, IStoreCollection destination, string destinationName, bool overwrite, IHttpContext httpContext)
        {
            return Task.FromResult(new StoreItemResult(DavStatusCode.PreconditionFailed));
        }

        /// <summary>
        /// The SupportsFastMove.
        /// </summary>
        /// <param name="destination">The destination<see cref="IStoreCollection"/>.</param>
        /// <param name="destinationName">The destinationName<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool SupportsFastMove(IStoreCollection destination, string destinationName, bool overwrite, IHttpContext httpContext)
        {
            return false;
        }

        /// <summary>
        /// The UploadFromStreamAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="source">The source<see cref="Stream"/>.</param>
        /// <returns>The <see cref="Task{DavStatusCode}"/>.</returns>
        public Task<DavStatusCode> UploadFromStreamAsync(IHttpContext httpContext, Stream source)
        {
            return Task.FromResult(DavStatusCode.Conflict);
        }

        public Task<bool> ServeGetRequest(IHttpContext httpContext)
        {
            httpContext.Response.SetStatus(DavStatusCode.PreconditionFailed);
            return Task.FromResult(true);
        }
    }
}
