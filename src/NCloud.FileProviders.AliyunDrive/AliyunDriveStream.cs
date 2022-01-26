// -----------------------------------------------------------------------
// <copyright file="AliyunDriveStream.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models;
    using NCloud.FileProviders.Support.Streams;

    /// <summary>
    /// Defines the <see cref="AliyunDriveStream" />.
    /// </summary>
    public class AliyunDriveStream : HttpStream
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
        /// Gets the Length.
        /// </summary>
        public override long Length => item.Size ?? Wait(() => GetLengthAsync());

        /// <summary>
        /// Initializes a new instance of the <see cref="AliyunDriveStream"/> class.
        /// </summary>
        /// <param name="item">The item<see cref="FileItem"/>.</param>
        /// <param name="client">The client<see cref="AliyunDriveClient"/>.</param>
        public AliyunDriveStream(FileItem item, AliyunDriveClient client) : base(null, new MemoryStream(), true, 1 << 18, null, null, null)
        {
            this.item = item;
            this.client = client;
            this.PrepareRequest = (req) =>
            {
                req.Headers.Add("referer", "https://www.aliyundrive.com/");
            };
        }

        /// <summary>
        /// The LoadAsync.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        protected override async Task<int> LoadAsync(Stream stream, int offset, int length, CancellationToken cancellationToken)
        {
            var res = await this.client.GetDownloadLinkAsync(item.FileId);
            this._uri = new Uri(res.Url);
            return await base.LoadAsync(stream, offset, length, cancellationToken);
        }
    }
}
