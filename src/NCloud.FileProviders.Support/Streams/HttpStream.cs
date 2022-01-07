// -----------------------------------------------------------------------
// <copyright file="HttpStream.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support.Streams
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Implements randomly accessible <see cref="Stream"/> on HTTP 1.1 transport.
    /// </summary>
    public partial class HttpStream : CacheStream
    {
        /// <summary>
        /// Defines the _uri.
        /// </summary>
        public readonly Uri _uri;

        /// <summary>
        /// Defines the _httpClient.
        /// </summary>
        public readonly HttpClient _httpClient;

        /// <summary>
        /// Defines the _ownHttpClient.
        /// </summary>
        public readonly bool _ownHttpClient;

        /// <summary>
        /// Defines the _bufferingSize.
        /// </summary>
        public int _bufferingSize;

        /// <summary>
        /// Gets the StreamLength
        /// Size in bytes of the file data downloaded so far if available; otherwise it returns <see cref="long.MaxValue"/>.
        /// <seealso cref="IsStreamLengthAvailable"/>
        /// <seealso cref="GetStreamLengthOrDefault"/>.
        /// </summary>
        public long StreamLength { get; private set; }

        /// <summary>
        /// Gets a value indicating whether InspectionFinished
        /// Whether file properties, like file size and last modified time is correctly inspected..
        /// </summary>
        public bool InspectionFinished { get; private set; }

        /// <summary>
        /// Gets the LastModified
        /// When the file is last modified..
        /// </summary>
        public DateTime LastModified { get; private set; }

        /// <summary>
        /// Gets the ContentType
        /// Content type of the file..
        /// </summary>
        public string? ContentType { get; private set; }

        /// <summary>
        /// Gets or sets the BufferingSize
        /// Buffering size for downloading the file..
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
        /// Initializes a new instance of the <see cref="HttpStream"/> class.
        /// </summary>
        /// <param name="uri">URI of the file to download.</param>
        [Obsolete("Please use the CreateAsync(Uri, CancellationToken) static method instead.")]
        public HttpStream(Uri uri) : this(uri, new MemoryStream(), true)
        {
        }

        /// <summary>
        /// Default cache page size; 32KB.
        /// </summary>
        public const int DefaultCachePageSize = 32 * 1024;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStream"/> class.
        /// </summary>
        /// <param name="uri">URI of the file to download.</param>
        /// <param name="cache">Stream, on which the file will be cached. It should be seekable, readable and writeable.</param>
        /// <param name="ownStream"><c>true</c> to dispose <paramref name="cache"/> on HttpStream's cleanup.</param>
        [Obsolete("Please use the CreateAsync(Uri, Stream, bool, CancellationToken) static method instead.")]
        public HttpStream(Uri uri, Stream cache, bool ownStream) : this(uri, cache, ownStream, DefaultCachePageSize, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStream"/> class.
        /// </summary>
        /// <param name="uri">URI of the file to download.</param>
        /// <param name="cache">Stream, on which the file will be cached. It should be seekable, readable and writeable.</param>
        /// <param name="ownStream"><c>true</c> to dispose <paramref name="cache"/> on HttpStream's cleanup.</param>
        /// <param name="cachePageSize">Cache page size.</param>
        /// <param name="cached">Cached flags for the pages in packed bits if any; otherwise it can be <c>null</c>.</param>
        [Obsolete("Please use the CreateAsync(Uri, Stream, bool, int, byte[]?, CancellationToken) static method instead.")]
        public HttpStream(Uri uri, Stream cache, bool ownStream, int cachePageSize, byte[]? cached)
            : this(uri, cache, ownStream, cachePageSize, cached, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStream"/> class.
        /// </summary>
        /// <param name="uri">URI of the file to download.</param>
        /// <param name="cache">Stream, on which the file will be cached. It should be seekable, readable and writeable.</param>
        /// <param name="ownStream"><c>true</c> to dispose <paramref name="cache"/> on HttpStream's cleanup.</param>
        /// <param name="cachePageSize">Cache page size.</param>
        /// <param name="cached">Cached flags for the pages in packed bits if any; otherwise it can be <c>null</c>.</param>
        /// <param name="httpClient"><see cref="HttpClient"/> to use on creating HTTP requests or <c>null</c> to use a default <see cref="HttpClient"/>.</param>
        [Obsolete("Please use the CreateAsync(Uri, Stream, bool, int, byte[]?, HttpClient?, CancellationToken) static method instead.")]
        public HttpStream(Uri uri, Stream cache, bool ownStream, int cachePageSize, byte[]? cached, HttpClient? httpClient)
            : this(uri, cache, ownStream, cachePageSize, cached, httpClient, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStream"/> class.
        /// </summary>
        /// <param name="uri">URI of the file to download.</param>
        /// <param name="cache">Stream, on which the file will be cached. It should be seekable, readable and writeable.</param>
        /// <param name="ownStream"><c>true</c> to dispose <paramref name="cache"/> on HttpStream's cleanup.</param>
        /// <param name="cachePageSize">Cache page size.</param>
        /// <param name="cached">Cached flags for the pages in packed bits if any; otherwise it can be <c>null</c>.</param>
        /// <param name="httpClient"><see cref="HttpClient"/> to use on creating HTTP requests or <c>null</c> to use a default <see cref="HttpClient"/>.</param>
        /// <param name="dispatcherInvoker">Function called on every call to synchronous <see cref="HttpStream.Read(byte[], int, int)"/> call to invoke <see cref="HttpStream.ReadAsync(byte[], int, int, CancellationToken)"/>.</param>
        [Obsolete("Please use the CreateAsync(Uri, Stream, bool, int, byte[]?, HttpClient?, DispatcherInvoker?, CancellationToken) static method instead.")]
        public HttpStream(Uri uri, Stream cache, bool ownStream, int cachePageSize, byte[]? cached, HttpClient? httpClient, DispatcherInvoker? dispatcherInvoker)
            : base(cache, ownStream, cachePageSize, cached, dispatcherInvoker)
        {
            StreamLength = long.MaxValue;
            _uri = uri;
            if (httpClient == null)
            {
                _httpClient = new HttpClient();
                _ownHttpClient = true;
            }
            else
            {
                _httpClient = httpClient;
                _ownHttpClient = false;
            }
            BufferingSize = cachePageSize;
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        /// <param name="disposing">The disposing<see cref="bool"/>.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && _ownHttpClient)
            {
                _httpClient.Dispose();
            }
        }

        /// <summary>
        /// Size in bytes of the file downloaing if available.
        /// </summary>
        /// <param name="defValue">If the file is not available, the value is returned.</param>
        /// <returns>The file size.</returns>
        public override long GetStreamLengthOrDefault(long defValue) => IsStreamLengthAvailable ? StreamLength : defValue;

        /// <summary>
        /// Gets or sets a value indicating whether IsStreamLengthAvailable
        /// Determine whether stream length is determined or not..
        /// </summary>
        public override bool IsStreamLengthAvailable { get; protected set; }

        /// <summary>
        /// Gets the LastHttpStatusCode
        /// Last HTTP status code..
        /// </summary>
        public System.Net.HttpStatusCode LastHttpStatusCode { get; private set; }

        /// <summary>
        /// Gets the LastReasonPhrase
        /// Last reason phrase obtained with <see cref="LastHttpStatusCode"/>..
        /// </summary>
        public string? LastReasonPhrase { get; private set; }

        /// <summary>
        /// Download a portion of file and write to a stream.
        /// </summary>
        /// <param name="stream">Stream to write on.</param>
        /// <param name="offset">The offset of the data to download.</param>
        /// <param name="length">The length of the data to download.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The byte range actually downloaded. It may be larger than the requested range.</returns>
        protected override async Task<int> LoadAsync(Stream stream, int offset, int length, CancellationToken cancellationToken)
        {
            if (length == 0)
            {
                return 0;
            }

            long endPos = offset + length;
            if (IsStreamLengthAvailable && endPos > StreamLength)
            {
                endPos = StreamLength;
            }

            var req = new HttpRequestMessage(HttpMethod.Get, _uri);
            // Use "Range" header to sepcify the data offset and size
            req.Headers.Add("Range", $"bytes={offset}-{endPos - 1}");

            // post the request
            var res = await _httpClient.SendAsync(req, cancellationToken).ConfigureAwait(false);
            LastHttpStatusCode = res.StatusCode;
            LastReasonPhrase = res.ReasonPhrase;
            if (!res.IsSuccessStatusCode)
            {
                throw new Exception($"HTTP Status: {res.StatusCode} for bytes={offset}-{endPos - 1}");
            }

            // retrieve the resulting Content-Range
            var getRanges = true;
            long begin = 0, end;
            var size = long.MaxValue;
            if (!ActionIfFound(res, "Content-Range", range =>
            {
                // 206
                var m = Regex.Match(range, @"bytes\s+([0-9]+)-([0-9]+)/(\w+)");
                begin = long.Parse(m.Groups[1].Value);
                end = long.Parse(m.Groups[2].Value);
                size = end - begin + 1;

                if (!IsStreamLengthAvailable)
                {
                    var sz = m.Groups[3].Value;
                    if (sz != "*")
                    {
                        StreamLength = long.Parse(sz);
                        IsStreamLengthAvailable = true;
                    }
                }
            }))
            {
                // In some case, there's no Content-Range but Content-Length
                // instead.
                getRanges = false;
                begin = 0;
                ActionIfFound(res, "Content-Length", v =>
                {
                    StreamLength = end = size = long.Parse(v);
                    IsStreamLengthAvailable = true;
                });
            }

            ActionIfFound(res, "Content-Type", v =>
            {
                ContentType = v;
            });

            ActionIfFound(res, "Last-Modified", v =>
            {
                LastModified = ParseDateTime(v);
            });

            InspectionFinished = true;

            var s = await res.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var size32 = (int)size;
            stream.Position = begin;
            var buf = new byte[BufferingSize];
            var copied = 0;
            while (size32 > 0)
            {
                var bytes2Read = Math.Min(size32, BufferingSize);
                var bytesRead = await s.ReadAsync(buf.AsMemory(0, bytes2Read), cancellationToken).ConfigureAwait(false);
                if (bytesRead <= 0)
                {
                    break;
                }

                await stream.WriteAsync(buf.AsMemory(0, bytesRead), cancellationToken).ConfigureAwait(false);
                size32 -= bytesRead;
                copied += bytesRead;
            }

            if (!IsStreamLengthAvailable && !getRanges)
            {
                StreamLength = copied;
                IsStreamLengthAvailable = true;
            }

            RangeDownloaded?.Invoke(this, new RangeDownloadedEventArgs { Offset = begin, Length = copied });
            return copied;
        }

        /// <summary>
        /// The actionIfFound.
        /// </summary>
        /// <param name="res">The res<see cref="HttpResponseMessage"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="action">The action<see cref="Action{string}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool ActionIfFound(HttpResponseMessage res, string name, Action<string> action)
        {
            if (res.Content.Headers.TryGetValues(name, out var strs))
            {
                action(strs.First());
                return true;
            }
            return false;
        }

        /// <summary>
        /// Invoked when a new range is downloaded.
        /// </summary>
        public event EventHandler<RangeDownloadedEventArgs>? RangeDownloaded;

        /// <summary>
        /// The parseDateTime.
        /// </summary>
        /// <param name="dateTime">The dateTime<see cref="string"/>.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        public static DateTime ParseDateTime(string dateTime)
        {
            if (dateTime.EndsWith(" UTC"))
            {
                return DateTime.ParseExact(dateTime,
                    "ddd, dd MMM yyyy HH:mm:ss 'UTC'",
                    CultureInfo.InvariantCulture.DateTimeFormat,
                    DateTimeStyles.AssumeUniversal);
            }
            return DateTime.ParseExact(dateTime,
                "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                CultureInfo.InvariantCulture.DateTimeFormat,
                DateTimeStyles.AssumeUniversal);
        }
    }

    /// <summary>
    /// Used by <see cref="HttpStream.RangeDownloaded"/> event.
    /// </summary>
    public class RangeDownloadedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the Offset
        /// The offset of the data downloaded..
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// Gets or sets the Length
        /// The length of the data downloaded..
        /// </summary>
        public long Length { get; set; }
    }
}
