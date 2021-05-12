// -----------------------------------------------------------------------
// <copyright file="VirtualFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Virtual
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
    using NCloud.Utils;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    /// <summary>
    /// Defines the <see cref="VirtualFileProvider" />.
    /// </summary>
    [FileProvider(Name = "virtual", Protocol = "virtual")]
    public class VirtualFileProvider : EmbeddableCompositeNCloudFileProvider
    {
        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualFileProvider"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        public VirtualFileProvider(IServiceProvider serviceProvider, string config, string prefix) : base(false, serviceProvider, config, prefix)
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var fileSettings = deserializer.Deserialize<List<FileSetting>>(setting.DecodeBase64());
            this.client = (HttpClient)serviceProvider.GetService(typeof(HttpClient)) ?? new HttpClient();
            var fileInfos = new List<IFileInfo> { new VirtualFileInfo(prefix) };
            this.BuildFileInfos(fileSettings, fileInfos, prefix);
            this.TryResolveEmbedded(fileInfos);
            var dict = fileInfos
                .Where(e => !(e is EmbeddedFileInfo))
                .Distinct(new LambdaEqual<IFileInfo>(e => e.GetVirtualOrPhysicalPath()))
                .ToDictionary((e) => e.GetVirtualOrPhysicalPath(), e => e);
            var inner = new VirtualInnerFileProvider(provider, dict, config, "");
            this.AddProvider(inner);
        }

        /// <summary>
        /// The BuildFileInfos.
        /// </summary>
        /// <param name="fileSettings">The fileSettings<see cref="List{FileSetting}"/>.</param>
        /// <param name="fileInfos">The fileInfos<see cref="List{IFileInfo}"/>.</param>
        /// <param name="basePath">The basePath<see cref="string"/>.</param>
        private void BuildFileInfos(List<FileSetting> fileSettings, List<IFileInfo> fileInfos, string basePath)
        {
            foreach (var fileSetting in fileSettings)
            {
                var info = ParseToInfo(fileSetting, basePath);
                if (info != null)
                {
                    fileInfos.Add(info);
                    if (fileSetting.Children != null && fileSetting.Children.Any())
                    {
                        BuildFileInfos(fileSetting.Children, fileInfos, basePath + "/" + fileSetting.Name);
                    }
                }
            }
        }

        /// <summary>
        /// The ParseToInfo.
        /// </summary>
        /// <param name="setting">The setting<see cref="FileSetting"/>.</param>
        /// <param name="basePath">The basePath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        private IFileInfo ParseToInfo(FileSetting setting, string basePath)
        {
            var path = basePath + "/" + setting.Name;
            if (setting.IsInvalid)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(setting.Content))
            {
                var memInfo = new InMemoryFileInfo(path, setting.Content, setting.Name, false);
                return new EmbeddedFileInfo(memInfo, path);
            }
            if (setting.HasChildren)
            {
                return new VirtualFileInfo(path);
            }
            else
            {
                return new RemoteFileInfo(setting.Url, new VirtualFileInfo(path, false), client);
            }
        }
    }
}
