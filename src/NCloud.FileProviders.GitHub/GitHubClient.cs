// -----------------------------------------------------------------------
// <copyright file="GitHubClient.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.GitHub
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

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
            var url = $"/repos/{owner}/{repo}/contents/{path}";
            var content = await client.GetStringAsync(url);
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
                    return (null, github, true);
                }
                else
                {
                    var list = JsonConvert.DeserializeObject<List<GitHubFileContent>>(content);
                    return (list, null, true);
                }
            }
        }
    }
}
