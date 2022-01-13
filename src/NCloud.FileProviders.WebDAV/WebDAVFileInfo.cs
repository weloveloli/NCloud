// -----------------------------------------------------------------------
// <copyright file="WebDAVFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.WebDAV
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Logging;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.Support.Logger;
    using NCloud.Utils;
    using WebDAVClient;
    using WebDAVClient.Model;

    /// <summary>
    /// Defines the <see cref="WebDAVFileInfo" />.
    /// </summary>
    public class WebDAVFileInfo : IFileInfo, IExtendedFileInfo, IVirtualPathFileInfo
    {
        /// <summary>
        /// Defines the item.
        /// </summary>
        private readonly Item item;

        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly IClient client;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<WebDAVFileInfo> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDAVFileInfo"/> class.
        /// </summary>
        /// <param name="item">The item<see cref="Item"/>.</param>
        /// <param name="webDAVClient">The webDAVClient<see cref="IClient"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{WebDAVFileProvider}"/>.</param>
        public WebDAVFileInfo(Item item, IClient webDAVClient)
        {
            this.item = item;
            this.client = webDAVClient;
            this.logger = ApplicationLogging.CreateLogger<WebDAVFileInfo>();
        }

        /// <summary>
        /// Gets a value indicating whether Exists.
        /// </summary>
        public bool Exists => true;

        /// <summary>
        /// Gets the Length.
        /// </summary>
        public long Length => item.ContentLength ?? 0;

        /// <summary>
        /// Gets the PhysicalPath.
        /// </summary>
        public string PhysicalPath => null;

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name => item.DisplayName;

        /// <summary>
        /// Gets the LastModified.
        /// </summary>
        public DateTimeOffset LastModified => item.LastModified.HasValue ? new DateTimeOffset(item.LastModified.Value) : DateTimeOffset.UtcNow;

        /// <summary>
        /// Gets a value indicating whether IsDirectory.
        /// </summary>
        public bool IsDirectory => false;

        /// <summary>
        /// Gets the ETag.
        /// </summary>
        public string ETag => item.Etag;

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream CreateReadStream()
        {
            this.logger.LogDebug("CreateReadStream for {name}", item.DisplayName);
            return new WebDAVStream(item, client);
        }

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <param name="endPosition">The endPosition<see cref="long?"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream CreateReadStream(long startPosition, long? endPosition = null)
        {
            Check.CheckIndex(startPosition, endPosition, item.ContentLength);
            return client.DownloadPartial(item.Href, startPosition, endPosition.GetValueOrDefault(this.Length)).Result;
        }

        /// <summary>
        /// The CreateReadStreamAsync.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public Task<Stream> CreateReadStreamAsync(CancellationToken cancellationToken = default)
        {
            this.logger.LogDebug("CreateReadStreamAsync for {name}", item.DisplayName);
            return Task.FromResult<Stream>(new WebDAVStream(item, client));
        }

        /// <summary>
        /// The CreateReadStreamAsync.
        /// </summary>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <param name="endPosition">The endPosition<see cref="long?"/>.</param>
        /// <param name="token">The token<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public Task<Stream> CreateReadStreamAsync(long startPosition, long? endPosition = null, CancellationToken token = default)
        {
            Check.CheckIndex(startPosition, endPosition, item.ContentLength);
            return client.DownloadPartial(item.Href, startPosition, endPosition.GetValueOrDefault(this.Length));
        }

        /// <summary>
        /// The GetVirtualPath.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetVirtualPath()
        {
            return item.Href;
        }
    }
}
