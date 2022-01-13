// -----------------------------------------------------------------------
// <copyright file="WebDAVFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.WebDAV
{
    using System;
    using System.Linq;
    using System.Net;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Logging;
    using NCloud.FileProviders.Support;
    using WebDAVClient;
    using WebDAVClient.Model;

    /// <summary>
    /// Defines the <see cref="WebDAVFileProvider" />.
    /// </summary>
    [FileProvider(Name = "webdav", Type = "webdav")]
    public class WebDAVFileProvider : PrefixNCloudFileProvider<WebDAVConfig>
    {
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<WebDAVFileProvider> logger;

        /// <summary>
        /// Defines the client.
        /// </summary>
        private IClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDAVFileProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="WebDAVConfig"/>.</param>
        public WebDAVFileProvider(IServiceProvider provider, WebDAVConfig config) : base(provider, config)
        {
            this.logger = provider.GetService<ILogger<WebDAVFileProvider>>();
            IClient c = new Client(new NetworkCredential { UserName = config.User, Password = config.Password });
            c.Server = config.Url;
            c.BasePath = config.BasePath;
            this.client = c;
        }

        /// <summary>
        /// The GetDirectoryContentsByRelPath.
        /// </summary>
        /// <param name="relPath">The relpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IDirectoryContents"/>.</returns>
        protected override IDirectoryContents GetDirectoryContentsByRelPath(string relPath)
        {
            this.logger.LogDebug("GetDirectoryContentsByRelPath {relpath}", relPath);
            var items = this.client.List(relPath).Result;
            return new EnumerableDirectoryContents(items.Select(e => this.ToFileInfo(e, relPath)));
        }

        /// <summary>
        /// The ToFileInfo.
        /// </summary>
        /// <param name="item">The item<see cref="Item"/>.</param>
        /// <param name="relPath">The relPath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        private IFileInfo ToFileInfo(Item item, string relPath)
        {
            if (item == null)
            {
                return null;
            }
            if (item.IsCollection)
            {
                return new VirtualFileInfo(relPath + "/" + item.DisplayName);
            }
            else
            {
                return new WebDAVFileInfo(item, client);
            }
        }

        /// <summary>
        /// The GetFileInfoByRelPath.
        /// </summary>
        /// <param name="relPath">The relPath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        protected override IFileInfo GetFileInfoByRelPath(string relPath)
        {
            var item = client.GetFile(relPath).Result;
            return this.ToFileInfo(item, relPath);
        }
    }
}
