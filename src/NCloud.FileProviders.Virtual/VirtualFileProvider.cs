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
    using NCloud.FileProviders.Support;
    using NCloud.Utils;
    using Newtonsoft.Json;

    /// <summary>
    /// Defines the <see cref="VirtualFileProvider" />.
    /// </summary>
    [FileProvider(Name = "virtual", Type = "virtual")]
    public class VirtualFileProvider : EmbeddableCompositeNCloudFileProvider<VirtualProviderConfig>
    {
        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly HttpClient client;

        /// <summary>
        /// Defines the converter.
        /// </summary>
        private readonly ProviderConfigConverter converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualFileProvider"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        public VirtualFileProvider(IServiceProvider serviceProvider, VirtualProviderConfig config) : base(serviceProvider, config)
        {
            this.converter = new ProviderConfigConverter();
            this.client = (HttpClient)serviceProvider.GetService(typeof(HttpClient)) ?? new HttpClient();
            var fileInfos = new List<IFileInfo> { new VirtualFileInfo(config.Prefix) };
            this.BuildFileInfos(config.FileSettings, fileInfos, config.Prefix);
            this.TryResolveEmbedded(fileInfos);
            var fileInfoTuples = fileInfos.Where(e => !(e is EmbeddedFileInfo))
                .Distinct(new LambdaEqual<IFileInfo>(e => e.GetVirtualOrPhysicalPath()))
                .Select(e => (e.GetVirtualOrPhysicalPath(), e)).ToList();
            var inner = new VirtualInterFileProvider(provider, new VirtualProviderConfig
            {
                FileSettings = config.FileSettings,
                Prefix = "",
                FileInfos = fileInfoTuples
            });
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
                var config = JsonConvert.DeserializeObject<BaseProviderConfig>(setting.Content, converter);
                if (config == null)
                {
                    return memInfo;
                }
                return new EmbeddedFileInfo(memInfo, config, path);
            }
            if (setting.HasChildren)
            {
                return new VirtualFileInfo(path);
            }
            else
            {
                return new HttpRemoteFileInfo(setting.Url, new VirtualFileInfo(path, false), client);
            }
        }
    }
}
