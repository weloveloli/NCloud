// -----------------------------------------------------------------------
// <copyright file="GitHubDrive.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Drives
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using NCloud.Core;
    using NCloud.Core.Attributes;
    using NCloud.Core.Model;
    using Newtonsoft.Json;
    using FileInfo = Core.Model.FileInfo;

    /// <summary>
    /// Defines the <see cref="GitHubDrive" />.
    /// </summary>
    [Drive(Name = "Github", Protocol = "github")]
    public class GitHubDrive : BaseDrive, IContentDrive
    {
        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly GitHubClient client;

        /// <summary>
        /// Defines the owner.
        /// </summary>
        private readonly string owner;

        /// <summary>
        /// Defines the project.
        /// </summary>
        private readonly string project;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubDrive"/> class.
        /// </summary>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="pathFromRoot">The pathFromRoot<see cref="string"/>.</param>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        public GitHubDrive(string config, string pathFromRoot, IServiceProvider serviceProvider) : base(config, pathFromRoot, serviceProvider)
        {
            var client = (HttpClient)serviceProvider.GetService(typeof(HttpClient));
            this.client = new GitHubClient(client);
            var setting = GetSetting();
            var settings = setting.Split("/");
            if (settings.Length != 2)
            {
                throw new ArgumentException($"invalid config: {config}");
            }
            this.owner = settings[0];
            this.project = settings[1];
        }

        /// <summary>
        /// The GetFileInfosByPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="List{FileInfo}"/>.</returns>
        public override async Task<NCloudResult> GetFileInfosByPathAsync(string path)
        {
            var infos = await this.client.GetFiles(path, this.owner, this.project);
            if (!infos.Item3)
            {
                throw new NCloudException(ResultEnum.File_Not_Found);
            }
            if (infos.Item1 != null)
            {
                return NCloudResult.OK(infos.Item1.Select(e => ToFileInfo(e, path)));
            }
            if (infos.Item2 != null)
            {
                return NCloudResult.OK(ToFileInfo(infos.Item2, path));
            }
            throw new NCloudException(ResultEnum.File_Not_Found);
        }

        /// <summary>
        /// The GetFileStream.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public async Task<Stream> GetFileStream(string path,bool c = true)
        {
            var cache = Path.GetTempFileName();
            var file = await this.client.GetFiles(path, this.owner, this.project);
            if (!file.Item3)
            {
                throw new NCloudException(ResultEnum.File_Not_Found);
            }
            if (file.Item2 == null)
            {
                throw new NCloudException(ResultEnum.File_Opt_Forbidden);
            }
            File.WriteAllText(cache, file.Item2.Content);
            return File.OpenRead(cache);
        }

        /// <summary>
        /// The ToFileInfo.
        /// </summary>
        /// <param name="content">The content<see cref="GitHubFileContent"/>.</param>
        /// <param name="relativePath">The relativePath<see cref="string"/>.</param>
        /// <returns>The <see cref="Core.Model.FileInfo"/>.</returns>
        private FileInfo ToFileInfo(GitHubFileContent content, string relativePath)
        {
            return new FileInfo
            {
                Name = content.Name,
                Size = content.Size,
                Type = content.Type,
                RemoteUrl = content.DownloadUrl,
                Path = string.IsNullOrEmpty(relativePath)? $"{this.pathFromRoot}/{content.Name}":$"{this.pathFromRoot}/{content.Path}"
            };
        }
    }

    /// <summary>
    /// Defines the <see cref="GitHubClient" />.
    /// </summary>
    public class GitHubClient
    {
        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubClient"/> class.
        /// </summary>
        /// <param name="httpClient">The httpClient<see cref="HttpClient"/>.</param>
        public GitHubClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://api.github.com/");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "NCloud");
            this.client = httpClient;
        }

        /// <summary>
        /// The GetData.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="owner">The owner<see cref="string"/>.</param>
        /// <param name="repo">The repo<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public async Task<(List<GitHubFileContent>, GitHubFileContent, bool)> GetFiles(string path, string owner, string repo)
        {
            var content = await client.GetStringAsync($"/repos/{owner}/{repo}/contents/{path}");
            if (string.IsNullOrEmpty(content))
            {
                return (null, null, false);
            }
            else
            {
                var isObject = content.StartsWith('{');
                if (isObject)
                {
                    var github = JsonConvert.DeserializeObject<GitHubFileContent>(content);
                    DecodeContent(github);
                    return (null, github, true);
                }
                else
                {
                    var list = JsonConvert.DeserializeObject<List<GitHubFileContent>>(content);
                    list.ForEach(github =>
                    {
                        DecodeContent(github);
                    });
                    return (list, null, true);
                }
            }
        }

        /// <summary>
        /// The DecodeContent.
        /// </summary>
        /// <param name="content">The content<see cref="GitHubFileContent"/>.</param>
        public static void DecodeContent(GitHubFileContent content)
        {
            if (!string.IsNullOrEmpty(content.Content))
            {
                byte[] c = Convert.FromBase64String(content.Content);
                var str = Encoding.Default.GetString(c);
                content.Content = str;
            }
        }
    }

    /// <summary>
    /// Defines the <see cref="GitHubFileContent" />.
    /// </summary>
    public class GitHubFileContent
    {
        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Size.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the Path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the HtmlUrl.
        /// </summary>
        [JsonProperty(PropertyName = "html_url")]
        public string HtmlUrl { get; set; }

        /// <summary>
        /// Gets or sets the DownloadUrl.
        /// </summary>
        [JsonProperty(PropertyName = "download_url")]
        public string DownloadUrl { get; set; }
    }
}
