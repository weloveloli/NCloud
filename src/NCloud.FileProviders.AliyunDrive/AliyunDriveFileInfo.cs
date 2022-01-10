// -----------------------------------------------------------------------
// <copyright file="AliyunDriveFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models;
    using NCloud.FileProviders.Support.Streams;

    /// <summary>
    /// Defines the <see cref="AliyunDriveFileInfo" />.
    /// </summary>
    public class AliyunDriveFileInfo : IExtendedFileInfo
    {
        /// <summary>
        /// Defines the item.
        /// </summary>
        private readonly FileItem item;

        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly AliyunDriveClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AliyunDriveFileInfo"/> class.
        /// </summary>
        /// <param name="item">The item<see cref="FileItem"/>.</param>
        /// <param name="client">The client<see cref="AliyunDriveClient"/>.</param>
        public AliyunDriveFileInfo(FileItem item, AliyunDriveClient client)
        {
            this.item = item;
            this.client = client;
        }

        /// <summary>
        /// Gets the ETag.
        /// </summary>
        public string ETag => item.Crc64Hash;

        /// <summary>
        /// Gets a value indicating whether Exists.
        /// </summary>
        public bool Exists => true;

        /// <summary>
        /// Gets the Length.
        /// </summary>
        public long Length => item.Size ?? 0;

        /// <summary>
        /// Gets the PhysicalPath.
        /// </summary>
        public string PhysicalPath => null;

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name => item.Name;

        /// <summary>
        /// Gets the LastModified.
        /// </summary>
        public DateTimeOffset LastModified => item.CreatedAt;

        /// <summary>
        /// Gets a value indicating whether IsDirectory.
        /// </summary>
        public bool IsDirectory => item.IsFolder;

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <param name="endPosition">The endPosition<see cref="long?"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream CreateReadStream(long startPosition, long? endPosition = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream CreateReadStream()
        {
            return new HttpStream(new Uri(item.DownloadUrl))
            {
                PrepareRequest = (req) =>
                {
                    req.Headers.Add("referer", "https://www.aliyundrive.com/");
                }
            };
        }

        /// <summary>
        /// The CreateReadStreamAsync.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public async Task<Stream> CreateReadStreamAsync(CancellationToken cancellationToken = default)
        {
            var stream = await HttpStream.CreateAsync(new Uri(item.DownloadUrl));
            stream.PrepareRequest = (req) =>
                {
                    req.Headers.Add("referer", "https://www.aliyundrive.com/");
                };
            return stream;
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
            throw new NotImplementedException();
        }
    }
}
