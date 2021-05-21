// -----------------------------------------------------------------------
// <copyright file="GithubFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.GitHub
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Support;

    /// <summary>
    /// Defines the <see cref="GithubFileProvider" />.
    /// </summary>
    [FileProvider(Name = "github", Protocol = "github")]
    public class GithubFileProvider : PrefixNCloudFileProvider
    {
        /// <summary>
        /// Defines the owner.
        /// </summary>
        private readonly string owner;

        /// <summary>
        /// Defines the project.
        /// </summary>
        private readonly string project;

        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly GitHubClient client;

        /// <summary>
        /// Defines the httpClient.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GithubFileProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        public GithubFileProvider(IServiceProvider provider, string config, string prefix) : base(provider, config, prefix)
        {
            var settings = setting.Split("/");
            if (settings.Length != 2)
            {
                throw new ArgumentException($"invalid config: {config}");
            }
            this.owner = settings[0];
            this.project = settings[1];
            this.httpClient = (HttpClient)provider.GetService(typeof(HttpClient)) ?? new HttpClient();
            this.client = (GitHubClient)provider.GetService(typeof(GitHubClient)) ?? new GitHubClient(this.httpClient);
        }

        /// <summary>
        /// The GetDirectoryContentsByRelPath.
        /// </summary>
        /// <param name="relpath">The relpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IDirectoryContents"/>.</returns>
        protected override IDirectoryContents GetDirectoryContentsByRelPath(string relpath)
        {
            var (list, item, exist) = this.client.GetFiles(relpath, this.owner, this.project).Result;
            if (!exist)
            {
                return null;
            }
            else
            {
                if (list != null && list.Any())
                {
                    var infos = list.Select(item => ToFileInfo(item)).ToList();
                    return new EnumerableDirectoryContents(infos);
                }
                return null;
            }
        }

        /// <summary>
        /// The GetFileInfoByRelPath.
        /// </summary>
        /// <param name="relPath">The relPath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        protected override IFileInfo GetFileInfoByRelPath(string relPath)
        {
            var (list, item, exist) = this.client.GetFiles(relPath, this.owner, this.project).Result;
            if (!exist)
            {
                return null;
            }
            else
            {
                if (list != null && list.Any())
                {
                    return new VirtualFileInfo(relPath);
                }
                if (item != null)
                {
                    return ToFileInfo(item);
                }
                return null;
            }
        }

        /// <summary>
        /// The ToFileInfo.
        /// </summary>
        /// <param name="item">The item<see cref="GitHubFileContent"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        private IFileInfo ToFileInfo(GitHubFileContent item)
        {
            if (item.Type == "file")
            {
                if (item.Content != null)
                {
                    return new HttpRemoteFileInfo(item.DownloadUrl, new InMemoryFileInfo(item.Path, item.Content, item.Name, true), httpClient);
                }
                else
                {
                    return new HttpRemoteFileInfo(item.DownloadUrl, new VirtualFileInfo(item.Path, false, item.Size), httpClient);
                }

            }
            else
            {
                return new VirtualFileInfo(item.Path);
            }
        }
    }
}
