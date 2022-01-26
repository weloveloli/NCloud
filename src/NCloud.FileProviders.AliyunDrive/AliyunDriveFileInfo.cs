// -----------------------------------------------------------------------
// <copyright file="AliyunDriveFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models;
    using NCloud.FileProviders.Support;
    using NCloud.FileProviders.Support.Logger;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="AliyunDriveFileInfo" />.
    /// </summary>
    public class AliyunDriveFileInfo : IExtendedFileInfo, IWebDAVGetHandler
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
        /// Defines the httpClient.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// the logger
        /// </summary>
        private readonly ILogger<AliyunDriveFileInfo> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AliyunDriveFileInfo"/> class.
        /// </summary>
        /// <param name="item">The item<see cref="FileItem"/>.</param>
        /// <param name="client">The client<see cref="AliyunDriveClient"/>.</param>
        /// <param name="httpClient">The httpClient<see cref="HttpClient"/>.</param>
        public AliyunDriveFileInfo(FileItem item, AliyunDriveClient client, HttpClient httpClient)
        {
            this.item = item;
            this.client = client;
            this.httpClient = httpClient;
            this.logger = ApplicationLogging.CreateLogger<AliyunDriveFileInfo>();
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
            logger.LogDebug("CreateReadStream startPosition {startPosition}, endPosition {endPosition}", startPosition, endPosition);
            Check.CheckIndex(startPosition, endPosition, this.Length);
            var downRes = this.client.GetDownloadLinkAsync(item.FileId).Result;
            Check.CheckIndex(startPosition, endPosition, this.Length);
            var request = new HttpRequestMessage { RequestUri = new Uri(downRes.Url) };
            request.Headers.Add("referer", "https://www.aliyundrive.com/");
            request.Headers.Range = new RangeHeaderValue(startPosition, endPosition);
            var res = httpClient.SendAsync(request).Result;
            res.EnsureSuccessStatusCode();
            return res.Content.ReadAsStreamAsync().Result;
        }

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream CreateReadStream()
        {
            return new AliyunDriveStream(item, client);
        }

        /// <summary>
        /// The CreateReadStreamAsync.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public Task<Stream> CreateReadStreamAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<Stream>(new AliyunDriveStream(item, client));
        }

        /// <summary>
        /// The CreateReadStreamAsync.
        /// </summary>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <param name="endPosition">The endPosition<see cref="long?"/>.</param>
        /// <param name="token">The token<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public async Task<Stream> CreateReadStreamAsync(long startPosition, long? endPosition = null, CancellationToken token = default)
        {
            logger.LogDebug("CreateReadStreamAsync startPosition {startPosition}, endPosition {endPosition}", startPosition, endPosition);
            Check.CheckIndex(startPosition, endPosition, this.Length);
            var downRes = await this.client.GetDownloadLinkAsync(item.FileId);
            var request = new HttpRequestMessage { RequestUri = new Uri(downRes.Url) };
            request.Headers.Range = new RangeHeaderValue(startPosition, endPosition);
            request.Headers.Add("referer", "https://www.aliyundrive.com/");
            var res = await httpClient.SendAsync(request, token);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// HandleWebDAVGetRequest
        /// </summary>
        /// <param name="httpContext">http context</param>
        /// <returns></returns>
        public async Task<bool> HandleWebDAVGetRequest(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return false;
            }
            var request = httpContext.Request;
            var response = httpContext.Response;
            var correlationId = response.Headers["x-correlation-id"].FirstOrDefault();
            foreach (var header in request.Headers)
            {
                logger.LogDebug("[{correlationId}] Webdav head[{}]:[{}]", correlationId, header.Key,string.Join(',',header.Value));
            }
            if (request.Method == "HEAD")
            {
                response.StatusCode = StatusCodes.Status200OK;
                response.ContentType = this.DetermineContentType();
                response.Headers["etag"] = this.CalculateEtag();
                response.ContentLength = this.Length;
                response.Headers["last-modified"] = this.LastModified.ToString("r", CultureInfo.GetCultureInfo("en-US"));
                response.Headers["date"] = DateTime.Now.ToString("r", CultureInfo.GetCultureInfo("en-US"));
                return true;
            }
            else
            {
                var downRes = await this.client.GetDownloadLinkAsync(item.FileId);
                var req = new HttpRequestMessage { RequestUri = new Uri(downRes.Url) };
                req.Headers.Add("referer", "https://www.aliyundrive.com/");
            
                if (request.Headers.ContainsKey("range") || request.Headers.ContainsKey("Range"))
                {
                    var rangeValue = request.Headers["range"];
                    if (rangeValue == StringValues.Empty)
                    {
                        rangeValue = request.Headers["Range"];
                    }
                    var range = rangeValue.ToString();
                    var split = range.Split("-");
                    if (split.Length == 2)
                    {
                        var end = split[1];
                        if (long.Parse(end) >= Length)
                        {
                            range = range.Substring(0, range.LastIndexOf('-'));
                        }
                    }
                    req.Headers.Add("range", range);
                }
                if (request.Headers.ContainsKey("if-range"))
                {
                    req.Headers.Add("if-range", request.Headers["if-range"].ToString());
                }
                var res = await httpClient.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
                response.StatusCode = (int)res.StatusCode;
                if(res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    response.Headers["accept-ranges"] = "bytes";
                    response.ContentLength = res.Content.Headers.ContentLength;
                    response.Headers["etag"] = this.ETag;
                    response.ContentType = res.Content.Headers.ContentType.ToString();
                    response.Headers["last-modified"] = this.LastModified.ToString("r", CultureInfo.GetCultureInfo("en-US"));
                    response.Headers["date"] = DateTime.Now.ToString("r", CultureInfo.GetCultureInfo("en-US"));
                }
                else if(res.StatusCode == System.Net.HttpStatusCode.PartialContent)
                {                    
                    response.Headers["accept-ranges"] = "bytes";
                    response.Headers["etag"] = this.ETag;
                    response.Headers["last-modified"] = this.LastModified.ToString("r", CultureInfo.GetCultureInfo("en-US"));
                    response.Headers["date"] = DateTime.Now.ToString("r", CultureInfo.GetCultureInfo("en-US"));
                    response.Headers["content-range"] = res.Content.Headers.ContentRange.ToString();
                    response.ContentLength = res.Content.Headers.ContentLength;
                    response.ContentType = res.Content.Headers.ContentType.ToString();
                }
                await res.Content.CopyToAsync(response.Body);
                return true;
            }
        }
    }
}
