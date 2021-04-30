// -----------------------------------------------------------------------
// <copyright file="VirtualDrive.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Drives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using NCloud.Core;
    using NCloud.Core.Attributes;
    using NCloud.Core.Model;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;
    using FileInfo = Core.Model.FileInfo;

    /// <summary>
    /// Defines the <see cref="VirtualDrive" />.
    /// </summary>
    [Drive(Name = "Virtual", Protocol = "virtual")]
    public class VirtualDrive : BaseDrive
    {
        /// <summary>
        /// Defines the helper.
        /// </summary>
        private readonly ISystemHelper helper;

        /// <summary>
        /// Defines the Root.
        /// </summary>
        private readonly FileSetting Root;

        /// <summary>
        /// Defines the driveFactory.
        /// </summary>
        private readonly IDriveFactory driveFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualDrive"/> class.
        /// </summary>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="pathFromRoot">The pathFromRoot<see cref="string"/>.</param>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        public VirtualDrive(string config, string pathFromRoot, IServiceProvider serviceProvider) : base(config, pathFromRoot, serviceProvider)
        {
            helper = (ISystemHelper)serviceProvider.GetService(typeof(ISystemHelper));
            driveFactory = (IDriveFactory)serviceProvider.GetService(typeof(IDriveFactory));
            var innerConfig = GetSetting();
            innerConfig = helper.DecodeBase64(innerConfig);
            var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var fileSettings = deserializer.Deserialize<List<FileSetting>>(innerConfig);
            if (!this.CheckNames(fileSettings))
            {
                throw new ArgumentException("config is not valid");
            }
            fileSettings = fileSettings.Where(e => TraversalAndBuildTree(e, "")).ToList();
            Root = new FileSetting
            {
                Children = fileSettings,
                Name = "",
            };
        }

        /// <summary>
        /// The TraversalTree.
        /// </summary>
        /// <param name="fileSetting">The fileSetting<see cref="FileSetting"/>.</param>
        /// <param name="relativepath">The relativepath<see cref="string"/>.</param>
        private bool TraversalAndBuildTree(FileSetting fileSetting, string relativepath)
        {
            if (!string.IsNullOrWhiteSpace(fileSetting.Content))
            {
                var drivePath = string.IsNullOrWhiteSpace(relativepath) ? $"{this.pathFromRoot}/{fileSetting.Name}" : $"{this.pathFromRoot}/{relativepath}/{fileSetting.Name}";
                return this.driveFactory.TryEnableDrive(drivePath, fileSetting.Content);
            }
            else
            {
                if (fileSetting.Children?.Any() ?? false)
                {
                    if (!this.CheckNames(fileSetting.Children))
                    {
                        return false;
                    }
                    fileSetting.Children = fileSetting.Children.Where(e => TraversalAndBuildTree(e, $"{relativepath}/{fileSetting.Name}")).ToList();
                    return true;
                }
                return true;
            }
        }

        /// <summary>
        /// The CheckNames.
        /// </summary>
        /// <param name="fileSettings">The fileSettings<see cref="List{FileSetting}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool CheckNames(List<FileSetting> fileSettings)
        {
            var distinctCount = fileSettings.Select(e => e.Name).Distinct().Count();
            return fileSettings.Count == distinctCount;
        }

        /// <summary>
        /// The GetFileInfosByPath.
        /// </summary>
        /// <param name="relativePath">The relativePath<see cref="string"/>.</param>
        /// <returns>The <see cref="List{FileInfo}"/>.</returns>
        public override Task<NCloudResult> GetFileInfosByPathAsync(string relativePath)
        {
            return Task.Run(() =>
            {
                FileSetting target = Root;
                var path = relativePath;
                while (!string.IsNullOrWhiteSpace(path) && target != null && target.Children != null && target.Children.Any())
                {
                    var subs = path.Split("/");
                    string cur = "";
                    if (subs.Length == 1)
                    {
                        cur = subs[0];
                        path = string.Empty;
                    }
                    else
                    {
                        cur = subs[0];
                        path = subs.Skip(1).Aggregate((a, b) => a + "/" + b);
                    }
                    target = target.Children.Where(e => e.Name == cur).FirstOrDefault();
                }
                if (!string.IsNullOrWhiteSpace(path) || target == null)
                {
                    return NCloudResult.Oops(ResultEnum.File_Not_Found);
                }
                else if (!(target.Children?.Any() ?? false))
                {
                    return NCloudResult.OK(this.ToFileInfo(target, relativePath));
                }
                else
                {
                    return NCloudResult.OK(target.Children.Select(e => this.ToFileInfo(e, relativePath)).OrderBy(e => e.Name));
                }
            });
        }

        /// <summary>
        /// The ToFileInfo.
        /// </summary>
        /// <param name="e">The e<see cref="FileSetting"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="FileInfo"/>.</returns>
        public FileInfo ToFileInfo(FileSetting e, string path)
        {
            var type = "file";
            var filePath = $"{this.pathFromRoot}/{path}";
            if (e.Children?.Any() ?? false || !string.IsNullOrWhiteSpace(e.Content))
            {
                type = "dir";
                filePath = $"{this.pathFromRoot}/{path}/{e.Name}";
            }
            return new FileInfo
            {
                Name = e.Name,
                Path = filePath,
                Size = 0,
                Type = type,
                RemoteUrl = e.Url
            };
        }
    }

    /// <summary>
    /// Defines the <see cref="FileSetting" />.
    /// </summary>
    public class FileSetting
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Children.
        /// </summary>
        public List<FileSetting> Children { get; set; }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        public string Content { get; set; }
    }
}
