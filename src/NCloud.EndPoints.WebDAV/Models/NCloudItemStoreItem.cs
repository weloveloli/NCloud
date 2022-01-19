// -----------------------------------------------------------------------
// <copyright file="NCloudItemStoreItem.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.WebDAV.Models
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Support;
    using NWebDav.Server;
    using NWebDav.Server.AspNetCore;
    using NWebDav.Server.Helpers;
    using NWebDav.Server.Http;
    using NWebDav.Server.Locking;
    using NWebDav.Server.Props;
    using NWebDav.Server.Stores;

    /// <summary>
    /// Defines the <see cref="NCloudItemStore" />.
    /// </summary>
    internal class NCloudItemStoreItem : IStoreItem, IRandomAccessStoreItem
    {
        /// <summary>
        /// Defines the fileInfo.
        /// </summary>
        private readonly IFileInfo fileInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudItemStoreItem"/> class.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <param name="lockingManager">The lockingManager<see cref="ILockingManager"/>.</param>
        public NCloudItemStoreItem(IFileInfo fileInfo, ILockingManager lockingManager)
        {
            this.fileInfo = fileInfo;
            this.LockingManager = lockingManager;
        }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name => this.fileInfo.Name;

        /// <summary>
        /// Gets the UniqueKey.
        /// </summary>
        public string UniqueKey => this.fileInfo.GetVirtualOrPhysicalPath();

        /// <summary>
        /// Gets the PropertyManager.
        /// </summary>
        public IPropertyManager PropertyManager => DefaultPropertyManager;

        /// <summary>
        /// Gets or sets the LockingManager
        /// Gets the LockingManager...
        /// </summary>
        public ILockingManager LockingManager { get; set; }

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
        /// The GetReadableStreamAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public Task<Stream> GetReadableStreamAsync(IHttpContext httpContext)
        {
            return this.fileInfo.CreateReadStreamAsync();
        }

        /// <summary>
        /// The UploadFromStreamAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="source">The source<see cref="Stream"/>.</param>
        /// <returns>The <see cref="Task{DavStatusCode}"/>.</returns>
        public Task<DavStatusCode> UploadFromStreamAsync(IHttpContext httpContext, Stream source)
        {
            return Task.FromResult(DavStatusCode.PreconditionFailed);
        }

        /// <summary>
        /// The SupportRangeAccess.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool SupportRangeAccess()
        {
            return this.fileInfo.IsRandomAccess();
        }

        /// <summary>
        /// The GetReadableStreamAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="startPoint">The startPoint<see cref="long"/>.</param>
        /// <param name="endPoint">The endPoint<see cref="long"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public Task<Stream> GetReadableStreamAsync(IHttpContext httpContext, long startPoint, long? endPoint)
        {
            return this.fileInfo.CreateReadStreamAsync(startPoint, endPoint);
        }

        /// <summary>
        /// The TotalLength.
        /// </summary>
        /// <returns>The <see cref="long"/>.</returns>
        public long TotalLength()
        {
            return this.fileInfo.Length;
        }

        public async Task<bool> ServeGetRequest(IHttpContext httpContext)
        {
            if (httpContext is AspNetCoreContext coreContext && this.fileInfo.SupportCustomHandleringWebDavGetRequest())
            {
                return await this.fileInfo.CustomHandleringWebDavGetRequest(coreContext.HttpContext).ConfigureAwait(false);
            }
            var request = httpContext.Request;
            var response = httpContext.Response;
            // ETag might be used for a conditional request
            string etag = null;

            // Add non-expensive headers based on properties
            var propertyManager = this.PropertyManager;
            if (propertyManager != null)
            {
                // Add Last-Modified header
                var lastModifiedUtc = (string)(await propertyManager.GetPropertyAsync(httpContext, this, DavGetLastModified<IStoreItem>.PropertyName, true).ConfigureAwait(false));
                if (lastModifiedUtc != null)
                {
                    response.SetHeaderValue("Last-Modified", lastModifiedUtc);
                }

                // Add ETag
                etag = (string)(await propertyManager.GetPropertyAsync(httpContext, this, DavGetEtag<IStoreItem>.PropertyName, true).ConfigureAwait(false));
                if (etag != null)
                {
                    response.SetHeaderValue("Etag", etag);
                }

                // Add type
                var contentType = (string)(await propertyManager.GetPropertyAsync(httpContext, this, DavGetContentType<IStoreItem>.PropertyName, true).ConfigureAwait(false));
                if (contentType != null)
                {
                    response.SetHeaderValue("Content-Type", contentType);
                }

                // Add language
                var contentLanguage = (string)(await propertyManager.GetPropertyAsync(httpContext, this, DavGetContentLanguage<IStoreItem>.PropertyName, true).ConfigureAwait(false));
                if (contentLanguage != null)
                {
                    response.SetHeaderValue("Content-Language", contentLanguage);
                }
            }

            await HandleStream(httpContext,  etag);
            return true;
        }

        /// <summary>
        /// The HandleStream.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="entry">The entry<see cref="IStoreItem"/>.</param>
        /// <param name="etag">The etag<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task HandleStream(IHttpContext httpContext, string etag)
        {
            // Obtain request and response
            var request = httpContext.Request;
            var response = httpContext.Response;
            // Determine if we are invoked as HEAD
            var head = request.HttpMethod == "HEAD";
            // Determine the requested range
            var range = request.GetRange();
            long? streamLength = null;
            // Stream the actual entry
            using (var stream = await this.GetReadableStreamAsync(httpContext).ConfigureAwait(false))
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
                            //response.SetHeaderValue("Transfer-Encoding", "chunked");

                            // Determine the total length
                            var length = stream.Length;

                            // Check if an 'If-Range' was specified
                            if (range?.If != null && this.PropertyManager != null)
                            {
                                var lastModifiedText = (string)await this.PropertyManager.GetPropertyAsync(httpContext, this, DavGetLastModified<IStoreItem>.PropertyName, true).ConfigureAwait(false);
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
                            streamLength = length;
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
                        await response.SendFileAsync(stream, range?.Start ?? 0, streamLength);
                    }
                    else
                    {
                        // Set the response
                        response.SetStatus(DavStatusCode.NoContent);
                    }
                }
            }
        }
        /// <summary>
        /// Gets the DefaultPropertyManager.
        /// </summary>
        public static PropertyManager<NCloudItemStoreItem> DefaultPropertyManager { get; } = new PropertyManager<NCloudItemStoreItem>(new DavProperty<NCloudItemStoreItem>[]
        {
            // RFC-2518 properties
            new DavCreationDate<NCloudItemStoreItem>
            {
                Getter = (context, item) => DateTime.UtcNow,
                Setter = (context, item, value) =>
                {
                    return DavStatusCode.Ok;
                }
            },
            new DavDisplayName<NCloudItemStoreItem>
            {
                Getter = (context, item) => item.fileInfo.Name
            },
            new DavGetContentLength<NCloudItemStoreItem>
            {
                Getter = (context, item) => item.fileInfo.Length
            },
            new DavGetContentType<NCloudItemStoreItem>
            {
                Getter = (context, item) => item.fileInfo.DetermineContentType()
            },
            new DavGetEtag<NCloudItemStoreItem>
            {
                // Calculating the Etag is an expensive operation,
                // because we need to scan the entire file.
                IsExpensive = true,
                Getter = (context, item) => item.fileInfo.CalculateEtag()
            },
            new DavGetLastModified<NCloudItemStoreItem>
            {
                Getter = (context, item) => item.fileInfo.LastModified.UtcDateTime,
                Setter = (context, item, value) =>
                {
                    return DavStatusCode.Ok;
                }
            },
            new DavGetResourceType<NCloudItemStoreItem>
            {
                Getter = (context, item) => null
            },

            // Default locking property handling via the LockingManager
            new DavLockDiscoveryDefault<NCloudItemStoreItem>(),
            new DavSupportedLockDefault<NCloudItemStoreItem>(),

            // Hopmann/Lippert collection properties
            // (although not a collection, the IsHidden property might be valuable)
            new DavExtCollectionIsHidden<NCloudItemStoreItem>
            {
                Getter = (context, item) => false
            },

            // Win32 extensions
            new Win32CreationTime<NCloudItemStoreItem>
            {
                Getter = (context, item) => item.fileInfo.LastModified.UtcDateTime,
                Setter = (context, item, value) =>
                {
                    return DavStatusCode.Ok;
                }
            },
            new Win32LastAccessTime<NCloudItemStoreItem>
            {
                Getter = (context, item) => item.fileInfo.LastModified.UtcDateTime,
                Setter = (context, item, value) =>
                {
                    return DavStatusCode.Ok;
                }
            },
            new Win32LastModifiedTime<NCloudItemStoreItem>
            {
                Getter = (context, item) => item.fileInfo.LastModified.UtcDateTime,
                Setter = (context, item, value) =>
                {
                    return DavStatusCode.Ok;
                }
            },
            new Win32FileAttributes<NCloudItemStoreItem>
            {
                Getter = (context, item) => FileAttributes.ReadOnly,
                Setter = (context, item, value) =>
                {
                    return DavStatusCode.Ok;
                }
            }
        });
    }
}
