// -----------------------------------------------------------------------
// <copyright file="HttpStream.CreateAsync.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support.Streams
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="HttpStream" />.
    /// </summary>
    public partial class HttpStream
    {
        /// <summary>
        /// Creates a new HttpStream with the specified URI.
        /// The file will be cached on memory.
        /// </summary>
        /// <param name="uri">URI of the file to download.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The <see cref="Task{HttpStream}"/>.</returns>
        public static async Task<HttpStream> CreateAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return await CreateAsync(uri, new MemoryStream(), true, cancellationToken);
        }

        /// <summary>
        /// Creates a new HttpStream with the specified URI.
        /// The file will be cached on memory.
        /// </summary>
        /// <param name="uri">URI of the file to download.</param>
        /// <param name="cachePageSize">The cachePageSize<see cref="int"/>.</param>
        /// <param name="prepareRequest">The prepareRequest<see cref="Action{HttpRequestMessage}"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The <see cref="Task{HttpStream}"/>.</returns>
        public static async Task<HttpStream> CreateAsync(Uri uri, int cachePageSize, Action<HttpRequestMessage> prepareRequest, CancellationToken cancellationToken = default)
        {
            return await CreateAsync(uri, new MemoryStream(), true, cachePageSize, null, null, null, prepareRequest, cancellationToken);
        }

        /// <summary>
        /// Creates a new HttpStream with the specified URI.
        /// </summary>
        /// <param name="uri">URI of the file to download.</param>
        /// <param name="cache">Stream, on which the file will be cached. It should be seekable, readable and writeable.</param>
        /// <param name="ownStream"><c>true</c> to dispose <paramref name="cache"/> on HttpStream's cleanup.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The <see cref="Task{HttpStream}"/>.</returns>
        public static async Task<HttpStream> CreateAsync(Uri uri, Stream cache, bool ownStream, CancellationToken cancellationToken = default)
        {
            return await CreateAsync(uri, cache, ownStream, DefaultCachePageSize, null, cancellationToken);
        }

        /// <summary>
        /// Creates a new HttpStream with the specified URI.
        /// </summary>
        /// <param name="uri">URI of the file to download.</param>
        /// <param name="cache">Stream, on which the file will be cached. It should be seekable, readable and writeable.</param>
        /// <param name="ownStream"><c>true</c> to dispose <paramref name="cache"/> on HttpStream's cleanup.</param>
        /// <param name="cachePageSize">Cache page size.</param>
        /// <param name="cached">Cached flags for the pages in packed bits if any; otherwise it can be <c>null</c>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The <see cref="Task{HttpStream}"/>.</returns>
        public static async Task<HttpStream> CreateAsync(Uri uri, Stream cache, bool ownStream, int cachePageSize, byte[]? cached, CancellationToken cancellationToken = default)
        {
            return await CreateAsync(uri, cache, ownStream, cachePageSize, cached, null, cancellationToken);
        }

        /// <summary>
        /// Creates a new HttpStream with the specified URI.
        /// </summary>
        /// <param name="uri">URI of the file to download.</param>
        /// <param name="cache">Stream, on which the file will be cached. It should be seekable, readable and writeable.</param>
        /// <param name="ownStream"><c>true</c> to dispose <paramref name="cache"/> on HttpStream's cleanup.</param>
        /// <param name="cachePageSize">Cache page size.</param>
        /// <param name="cached">Cached flags for the pages in packed bits if any; otherwise it can be <c>null</c>.</param>
        /// <param name="httpClient"><see cref="HttpClient"/> to use on creating HTTP requests or <c>null</c> to use a default <see cref="HttpClient"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The <see cref="Task{HttpStream}"/>.</returns>
        public static async Task<HttpStream> CreateAsync(Uri uri, Stream cache, bool ownStream, int cachePageSize, byte[]? cached, HttpClient? httpClient, CancellationToken cancellationToken = default)
        {
            return await CreateAsync(uri, cache, ownStream, cachePageSize, cached, httpClient, null, null, cancellationToken);
        }

        /// <summary>
        /// Creates a new HttpStream with the specified URI.
        /// </summary>
        /// <param name="uri">URI of the file to download.</param>
        /// <param name="cache">Stream, on which the file will be cached. It should be seekable, readable and writeable.</param>
        /// <param name="ownStream"><c>true</c> to dispose <paramref name="cache"/> on HttpStream's cleanup.</param>
        /// <param name="cachePageSize">Cache page size.</param>
        /// <param name="cached">Cached flags for the pages in packed bits if any; otherwise it can be <c>null</c>.</param>
        /// <param name="httpClient"><see cref="HttpClient"/> to use on creating HTTP requests or <c>null</c> to use a default <see cref="HttpClient"/>.</param>
        /// <param name="dispatcherInvoker">Function called on every call to synchronous <see cref="HttpStream.Read(byte[], int, int)"/> call to invoke <see cref="HttpStream.ReadAsync(byte[], int, int, CancellationToken)"/>.</param>
        /// <param name="prepareRequest">The prepareRequest<see cref="Action{HttpRequestMessage}"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The <see cref="Task{HttpStream}"/>.</returns>
        public static async Task<HttpStream> CreateAsync(Uri uri, Stream cache, bool ownStream, int cachePageSize, byte[]? cached, HttpClient? httpClient, DispatcherInvoker? dispatcherInvoker, Action<HttpRequestMessage> prepareRequest = null, CancellationToken cancellationToken = default)
        {
#pragma warning disable 618
            var httpStream = new HttpStream(uri, cache, ownStream, cachePageSize, cached, httpClient, dispatcherInvoker)
            {
                PrepareRequest = prepareRequest
            };

#pragma warning restore 618
            var req = new HttpRequestMessage(HttpMethod.Head, uri);
            if (prepareRequest != null)
            {
                prepareRequest.Invoke(req);
            }
            using var headResponse = await httpStream._httpClient.SendAsync(req, cancellationToken);
            var contentLength = headResponse.Content.Headers.ContentLength;
            if (contentLength.HasValue)
            {
                httpStream.StreamLength = contentLength.Value;
                httpStream.IsStreamLengthAvailable = true;
            }
            return httpStream;
        }
    }
}
