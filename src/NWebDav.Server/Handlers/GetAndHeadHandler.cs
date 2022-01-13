﻿// -----------------------------------------------------------------------
// <copyright file="GetAndHeadHandler.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Handlers
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using NWebDav.Server.Helpers;
    using NWebDav.Server.Http;
    using NWebDav.Server.Logging;
    using NWebDav.Server.Props;
    using NWebDav.Server.Stores;

    /// <summary>
    /// Implementation of the GET and HEAD method.
    /// </summary>
    public class GetAndHeadHandler : IRequestHandler
    {
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAndHeadHandler"/> class.
        /// </summary>
        public GetAndHeadHandler()
        {
            this.logger = LoggerFactory.CreateLogger(typeof(GetAndHeadHandler));
        }

        /// <summary>
        /// Handle a GET or HEAD request.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="store">The store<see cref="IStore"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> HandleRequestAsync(IHttpContext httpContext, IStore store)
        {
            // Obtain request and response
            var request = httpContext.Request;
            var response = httpContext.Response;

            // Determine if we are invoked as HEAD
            var head = request.HttpMethod == "HEAD";

            // Determine the requested range
            var range = request.GetRange();

            // Obtain the WebDAV collection
            var entry = await store.GetItemAsync(request.Url, httpContext).ConfigureAwait(false);
            if (entry == null)
            {
                // Set status to not found
                response.SetStatus(DavStatusCode.NotFound);
                return true;
            }

            // ETag might be used for a conditional request
            string etag = null;

            // Add non-expensive headers based on properties
            var propertyManager = entry.PropertyManager;
            if (propertyManager != null)
            {
                // Add Last-Modified header
                var lastModifiedUtc = (string)(await propertyManager.GetPropertyAsync(httpContext, entry, DavGetLastModified<IStoreItem>.PropertyName, true).ConfigureAwait(false));
                if (lastModifiedUtc != null)
                {
                    response.SetHeaderValue("Last-Modified", lastModifiedUtc);
                }

                // Add ETag
                etag = (string)(await propertyManager.GetPropertyAsync(httpContext, entry, DavGetEtag<IStoreItem>.PropertyName, true).ConfigureAwait(false));
                if (etag != null)
                {
                    response.SetHeaderValue("Etag", etag);
                }

                // Add type
                var contentType = (string)(await propertyManager.GetPropertyAsync(httpContext, entry, DavGetContentType<IStoreItem>.PropertyName, true).ConfigureAwait(false));
                if (contentType != null)
                {
                    response.SetHeaderValue("Content-Type", contentType);
                }

                // Add language
                var contentLanguage = (string)(await propertyManager.GetPropertyAsync(httpContext, entry, DavGetContentLanguage<IStoreItem>.PropertyName, true).ConfigureAwait(false));
                if (contentLanguage != null)
                {
                    response.SetHeaderValue("Content-Language", contentLanguage);
                }
            }
            if (entry is IRandomAccessStoreItem randomAccessStoreItem && randomAccessStoreItem.SupportRangeAccess())
            {
                await HandleRandomAccessStream(httpContext, entry, etag);
            }
            else
            {
                await HandleStream(httpContext, entry, etag);
            }
            return true;
        }

        /// <summary>
        /// The HandleRandomAccessStream.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="entry">The entry<see cref="IStoreItem"/>.</param>
        /// <param name="etag">The etag<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task HandleRandomAccessStream(IHttpContext httpContext, IStoreItem entry, string etag)
        {
            this.logger.Log(LogLevel.Debug, () => $"HandleRandomAccessStream");
            var item = entry as IRandomAccessStoreItem;
            // Obtain request and response
            var request = httpContext.Request;
            var response = httpContext.Response;
            var propertyManager = entry.PropertyManager;
            // Determine if we are invoked as HEAD
            var head = request.HttpMethod == "HEAD";

            // Determine the requested range
            var range = request.GetRange();
            // Determine the total length
            var length = item.TotalLength();
            // Set the response
            response.SetStatus(DavStatusCode.Ok);
            // Add a header that we accept ranges (bytes only)
            response.SetHeaderValue("Accept-Ranges", "bytes");
            // Set the header, so the client knows how much data is required
            response.SetHeaderValue("Content-Length", $"{length}");
            // Do not return the actual item data if ETag matches
            if (etag != null && request.GetHeaderValue("If-None-Match") == etag)
            {
                response.SetHeaderValue("Content-Length", "0");
                response.SetStatus(DavStatusCode.NotModified);
            }
            if (head)
            {
                return;
            }
            // Check if an 'If-Range' was specified
            if (range?.If != null && propertyManager != null)
            {
                var lastModifiedText = (string)await propertyManager.GetPropertyAsync(httpContext, entry, DavGetLastModified<IStoreItem>.PropertyName, true).ConfigureAwait(false);
                var lastModified = DateTime.Parse(lastModifiedText, CultureInfo.InvariantCulture);
                if (lastModified != range.If)
                {
                    range = null;
                }
            }
            // Check if a range was specified
            if (range != null)
            {
                var start = range.Start ?? 0;
                var end = Math.Min(range.End ?? long.MaxValue, length - 1);
                length = end - start + 1;

                // Write the range
                response.SetHeaderValue("Content-Range", $"bytes {start}-{end} / {item.TotalLength()}");

                // Set status to partial result if not all data can be sent
                if (length < item.TotalLength())
                {
                    response.SetStatus(DavStatusCode.PartialContent);
                }
            }
            this.logger.Log(LogLevel.Debug, () => $"HandleRandomAccessStream, start {range?.Start}, end {range?.End}");
            using (var stream = await item.GetReadableStreamAsync(httpContext, range?.Start ?? 0, range?.End))
            {
                if (stream != null && stream != Stream.Null)
                {
                    await CopyToAsync(stream, response.Stream, 0, stream.Length - 1).ConfigureAwait(false);
                }
                else
                {
                    // Set the response
                    response.SetStatus(DavStatusCode.NoContent);
                }
            }
        }

        /// <summary>
        /// The HandleStream.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="entry">The entry<see cref="IStoreItem"/>.</param>
        /// <param name="etag">The etag<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task HandleStream(IHttpContext httpContext, IStoreItem entry, string etag)
        {
            this.logger.Log(LogLevel.Debug, () => $"HandleStream");
            // Obtain request and response
            var request = httpContext.Request;
            var response = httpContext.Response;
            var propertyManager = entry.PropertyManager;
            // Determine if we are invoked as HEAD
            var head = request.HttpMethod == "HEAD";
            // Determine the requested range
            var range = request.GetRange();
            // Stream the actual entry
            using (var stream = await entry.GetReadableStreamAsync(httpContext).ConfigureAwait(false))
            {
                if (stream != null && stream != Stream.Null)
                {
                    // Set the response
                    response.SetStatus(DavStatusCode.Ok);

                    // Set the expected content length
                    try
                    {
                        // We can only specify the Content-Length header if the
                        // length is known (this is typically true for seekable streams)
                        if (stream.CanSeek)
                        {
                            // Add a header that we accept ranges (bytes only)
                            response.SetHeaderValue("Accept-Ranges", "bytes");

                            // Determine the total length
                            var length = stream.Length;

                            // Check if an 'If-Range' was specified
                            if (range?.If != null && propertyManager != null)
                            {
                                var lastModifiedText = (string)await propertyManager.GetPropertyAsync(httpContext, entry, DavGetLastModified<IStoreItem>.PropertyName, true).ConfigureAwait(false);
                                var lastModified = DateTime.Parse(lastModifiedText, CultureInfo.InvariantCulture);
                                if (lastModified != range.If)
                                {
                                    range = null;
                                }
                            }

                            // Check if a range was specified
                            if (range != null)
                            {
                                var start = range.Start ?? 0;
                                var end = Math.Min(range.End ?? long.MaxValue, length - 1);
                                length = end - start + 1;

                                // Write the range
                                response.SetHeaderValue("Content-Range", $"bytes {start}-{end} / {stream.Length}");

                                // Set status to partial result if not all data can be sent
                                if (length < stream.Length)
                                {
                                    response.SetStatus(DavStatusCode.PartialContent);
                                }
                            }

                            // Set the header, so the client knows how much data is required
                            response.SetHeaderValue("Content-Length", $"{length}");
                        }
                    }
                    catch (NotSupportedException)
                    {
                        // If the content length is not supported, then we just skip it
                    }

                    // Do not return the actual item data if ETag matches
                    if (etag != null && request.GetHeaderValue("If-None-Match") == etag)
                    {
                        response.SetHeaderValue("Content-Length", "0");
                        response.SetStatus(DavStatusCode.NotModified);
                        return;
                    }

                    // HEAD method doesn't require the actual item data
                    if (!head)
                    {
                        await CopyToAsync(stream, response.Stream, range?.Start ?? 0, range?.End).ConfigureAwait(false);
                    }
                }
                else
                {
                    // Set the response
                    response.SetStatus(DavStatusCode.NoContent);
                }
            }
        }

        /// <summary>
        /// The CopyToAsync.
        /// </summary>
        /// <param name="src">The src<see cref="Stream"/>.</param>
        /// <param name="dest">The dest<see cref="Stream"/>.</param>
        /// <param name="start">The start<see cref="long"/>.</param>
        /// <param name="end">The end<see cref="long?"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task CopyToAsync(Stream src, Stream dest, long start, long? end)
        {
            // Skip to the first offset
            if (start > 0)
            {
                // We prefer seeking instead of draining data
                if (!src.CanSeek)
                {
                    throw new IOException("Cannot use range, because the source stream isn't seekable");
                }

                src.Seek(start, SeekOrigin.Begin);
            }

            // Determine the number of bytes to read
            var bytesToRead = end - start + 1 ?? long.MaxValue;

            // Read in 64KB blocks
            var buffer = new byte[64 * 1024];

            // Copy, until we don't get any data anymore
            while (bytesToRead > 0)
            {
                // Read the requested bytes into memory
                var requestedBytes = (int)Math.Min(bytesToRead, buffer.Length);
                var bytesRead = await src.ReadAsync(buffer, 0, requestedBytes).ConfigureAwait(false);

                // We're done, if we cannot read any data anymore
                if (bytesRead == 0)
                {
                    return;
                }

                // Write the data to the destination stream
                await dest.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);

                // Decrement the number of bytes left to read
                bytesToRead -= bytesRead;
            }
        }
    }
}
