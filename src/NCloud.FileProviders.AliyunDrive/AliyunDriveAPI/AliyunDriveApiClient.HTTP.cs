// -----------------------------------------------------------------------
// <copyright file="AliyunDriveApiClient.HTTP.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AliyunDriveApiClient" />.
    /// </summary>
    public partial class AliyunDriveApiClient
    {
        /// <summary>
        /// The SendJsonPostAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="obj">The obj<see cref="JsonNode"/>.</param>
        /// <param name="prepareToken">The prepareToken<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        private async Task<T> SendJsonPostAsync<T>(string url, JsonNode obj, bool prepareToken = true)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (prepareToken)
                await PrepareTokenAsync();
            var content = new StringContent(obj.ToJsonString(), Encoding.UTF8, "application/json");
            var resp = await _httpClient.PostAsync(url, content);
            var json = await TryThrowExceptionAndReadContentAsync(url, resp);
            return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
        }

        /// <summary>
        /// The SendJsonPostAsync.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="obj">The obj<see cref="JsonNode"/>.</param>
        /// <param name="prepareToken">The prepareToken<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task SendJsonPostAsync(string url, JsonNode obj, bool prepareToken = true)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (prepareToken)
                await PrepareTokenAsync();
            var content = new StringContent(obj.ToJsonString(), Encoding.UTF8, "application/json");
            var resp = await _httpClient.PostAsync(url, content);
            await TryThrowExceptionAndReadContentAsync(url, resp);
        }

        /// <summary>
        /// The SendJsonPostAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <param name="prepareToken">The prepareToken<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        private async Task<T> SendJsonPostAsync<T>(string url, object obj, bool prepareToken = true)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
            if (prepareToken)
                await PrepareTokenAsync();
            string body = obj == null ? "{}" : JsonSerializer.Serialize(obj, JsonSerializerOptions);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var resp = await _httpClient.PostAsync(url, content);
            var json = await TryThrowExceptionAndReadContentAsync(url, resp);
            return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
        }

        /// <summary>
        /// The SendJsonPostAsync.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <param name="prepareToken">The prepareToken<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task SendJsonPostAsync(string url, object obj, bool prepareToken = true)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
            if (prepareToken)
                await PrepareTokenAsync();
            string body = obj == null ? "{}" : JsonSerializer.Serialize(obj, JsonSerializerOptions);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var resp = await _httpClient.PostAsync(url, content);
            await TryThrowExceptionAndReadContentAsync(url, resp);
        }
    }
}
