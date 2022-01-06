// -----------------------------------------------------------------------
// <copyright file="WebDAVStream.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.WebDAV
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NCloud.FileProviders.Support.Streams;
    using WebDAVClient;
    using WebDAVClient.Model;

    /// <summary>
    /// Defines the <see cref="WebDAVStream" />.
    /// </summary>
    public class WebDAVStream : CacheStream
    {
        /// <summary>
        /// Defines the client.
        /// </summary>
        private IClient client;

        /// <summary>
        /// Defines the item.
        /// </summary>
        private Item item;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private ILogger<WebDAVFileProvider> logger;

        /// <summary>
        /// Defines the _bufferingSize.
        /// </summary>
        private int _bufferingSize;

        /// <summary>
        /// Buffering size for downloading the file.
        /// </summary>
        public int BufferingSize {
            get => _bufferingSize;
            set {
                if (value == 0 || BitCount(value) != 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(BufferingSize), value, "BufferingSize should be 2^n.");
                }

                _bufferingSize = value;
            }
        }

        /// <summary>
        /// The bitCount.
        /// </summary>
        /// <param name="i">The i<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int BitCount(int i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }

        /// <summary>
        /// Defines the streamLengthAvailable.
        /// </summary>
        private bool streamLengthAvailable;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDAVStream"/> class.
        /// </summary>
        /// <param name="item">The item<see cref="Item"/>.</param>
        /// <param name="client">The client<see cref="IClient"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{WebDAVFileProvider}"/>.</param>
        /// <param name="cachePageSize">The cachePageSize<see cref="int"/>.</param>
        public WebDAVStream(Item item, IClient client, ILogger<WebDAVFileProvider> logger, int cachePageSize = 256 * 1024 ) : base(new MemoryStream(), true, cachePageSize)
        {
            this.client = client;
            this.item = item;
            this.logger = logger;
            this.BufferingSize = cachePageSize;
            streamLengthAvailable = item.ContentLength.HasValue;
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsStreamLengthAvailable.
        /// </summary>
        public override bool IsStreamLengthAvailable { get => streamLengthAvailable; protected set => streamLengthAvailable = value; }

        /// <summary>
        /// The GetStreamLengthOrDefault.
        /// </summary>
        /// <param name="defValue">The defValue<see cref="long"/>.</param>
        /// <returns>The <see cref="long"/>.</returns>
        public override long GetStreamLengthOrDefault(long defValue)
        {
            return item.ContentLength.GetValueOrDefault(defValue);
        }

        /// <summary>
        /// The LoadAsync.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        protected async override Task<int> LoadAsync(Stream stream, int offset, int length, CancellationToken cancellationToken)
        {
            logger.LogDebug("Load Async offset:{} length:{}", offset, length);
            if (offset >= this.Length)
            {
                return 0;
            }
            var endPoint = offset + length > this.Length ? this.Length - 1 : offset + length - 1;
            var downloadStream = await client.DownloadPartial(item.Href, offset, offset + length - 1);
            var size32 = (int)(endPoint - offset + 1);
            stream.Position = offset;
            var buf = new byte[BufferingSize];
            var copied = 0;
            while (size32 > 0)
            {
                var bytes2Read = Math.Min(size32, BufferingSize);
                var bytesRead = await downloadStream.ReadAsync(buf.AsMemory(0, bytes2Read), cancellationToken).ConfigureAwait(false);
                if (bytesRead <= 0)
                {
                    break;
                }

                await stream.WriteAsync(buf.AsMemory(0, bytesRead), cancellationToken).ConfigureAwait(false);
                size32 -= bytesRead;
                copied += bytesRead;
            }
            return copied;
        }
    }
}
