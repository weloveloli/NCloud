// -----------------------------------------------------------------------
// <copyright file="HttpRemoteFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.Support.Streams;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="IRemoteFileInfo" />.
    /// </summary>
    public class HttpRemoteFileInfo : FileInfoDecorator, IExtendedFileInfo
    {
        /// <summary>
        /// Defines the contentLength.
        /// </summary>
        private long? contentLength;

        /// <summary>
        /// Gets or sets the RemoteUrl.
        /// </summary>
        public string RemoteUrl { get; set; }

        /// <summary>
        /// Defines the client.
        /// </summary>
        private HttpClient client;

        /// <summary>
        /// Defines the supportRange.
        /// </summary>
        private bool supportRange;

        /// <summary>
        /// Gets the Length.
        /// </summary>
        public override long Length => contentLength ?? base.Length;

        /// <summary>
        /// Defines the etag.
        /// </summary>
        private string etag;

        /// <summary>
        /// Gets the ETag.
        /// </summary>
        public string ETag => etag;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRemoteFileInfo"/> class.
        /// </summary>
        /// <param name="remoteUrl">The remoteUrl<see cref="string"/>.</param>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <param name="client">The client<see cref="HttpClient"/>.</param>
        /// <param name="checkLength">The checkLength<see cref="bool"/>.</param>
        /// <param name="supportRange">The supportRange<see cref="bool"/>.</param>
        /// <param name="etag">The etag<see cref="string"/>.</param>
        public HttpRemoteFileInfo(string remoteUrl, IFileInfo fileInfo, HttpClient client, bool checkLength = false, bool supportRange = true, string etag = null) : base(fileInfo)
        {
            this.RemoteUrl = remoteUrl;
            this.client = client;
            this.supportRange = supportRange;
            if (client != null && checkLength)
            {
                var res = client.GetAsync(remoteUrl, HttpCompletionOption.ResponseHeadersRead).Result;
                var contentLength = res.Content.Headers.GetValues("Content-Length").FirstOrDefault();
                this.contentLength = contentLength == null ? null : long.Parse(contentLength);
                etag ??= res.Content.Headers.GetValues("ETag").FirstOrDefault();              
            }
            if (etag == null)
            {
                var hash = SHA256.Create().ComputeHash(remoteUrl.GetBytes());
                etag = BitConverter.ToString(hash).Replace("-", string.Empty);
            }
            this.etag = etag;
        }

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <returns>The <see cref="Stream"/>.</returns>
        public override Stream CreateReadStream()
        {
            var stream = this.InnerIFileInfo.CreateReadStream();
            if (stream == null && client != null)
            {
                if (supportRange)
                {
                    stream = new HttpStream(new Uri(RemoteUrl));
                }
                else
                {
                    stream = client.GetStreamAsync(RemoteUrl).Result;
                }

            }
            return stream;
        }

        /// <summary>
        /// The OpenRead.
        /// </summary>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <param name="endPosition">The endPosition<see cref="long?"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream CreateReadStream(long startPosition, long? endPosition)
        {
            if (!supportRange && !(InnerIFileInfo is IExtendedFileInfo))
            {
                throw new IOException("not support random access");
            }
            if (InnerIFileInfo is IExtendedFileInfo randomAccessFileInfo)
            {
                return randomAccessFileInfo.CreateReadStream(startPosition, endPosition);
            }

            Check.CheckIndex(startPosition, endPosition, this.Length);
            var request = new HttpRequestMessage { RequestUri = new Uri(this.RemoteUrl) };
            request.Headers.Range = new RangeHeaderValue(startPosition, endPosition);
            var res = client.SendAsync(request).Result;
            res.EnsureSuccessStatusCode();
            return res.Content.ReadAsStream();
        }

        /// <summary>
        /// The CreateReadStreamAsync.
        /// </summary>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <param name="endPosition">The endPosition<see cref="long?"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public Task<Stream> CreateReadStreamAsync(long startPosition, long? endPosition = null)
        {
            return this.CreateReadStreamAsync(startPosition, endPosition, CancellationToken.None);
        }

        /// <summary>
        /// The CreateReadStreamAsync.
        /// </summary>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <param name="endPosition">The endPosition<see cref="long?"/>.</param>
        /// <param name="token">The token<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public async Task<Stream> CreateReadStreamAsync(long startPosition, long? endPosition, CancellationToken token)
        {
            if (!supportRange && !(InnerIFileInfo is IExtendedFileInfo))
            {
                throw new IOException("not support random access");
            }
            if (InnerIFileInfo is IExtendedFileInfo randomAccessFileInfo)
            {
                return await randomAccessFileInfo.CreateReadStreamAsync(startPosition, endPosition, token);
            }
            Check.CheckIndex(startPosition, endPosition, this.Length);
            var request = new HttpRequestMessage { RequestUri = new Uri(this.RemoteUrl) };
            request.Headers.Range = new RangeHeaderValue(startPosition, endPosition);
            var res = await client.SendAsync(request, token);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStreamAsync(token);
        }

        /// <summary>
        /// The CreateReadStreamAsync.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public async Task<Stream> CreateReadStreamAsync(CancellationToken cancellationToken = default)
        {
            return await HttpStream.CreateAsync(new Uri(RemoteUrl), cancellationToken);
        }
    }
}
