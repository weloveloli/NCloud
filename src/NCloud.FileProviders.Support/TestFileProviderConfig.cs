// -----------------------------------------------------------------------
// <copyright file="TestFileProviderConfig.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="TestFileProviderConfig" />.
    /// </summary>
    public class TestFileProviderConfig : BaseProviderConfig, IDictionaryBasedProviderConfig
    {
        /// <summary>
        /// Gets or sets the Settings.
        /// </summary>
        public string Settings { get; set; }

        /// <summary>
        /// Gets the Type.
        /// </summary>
        public override string Type => "test";

        /// <summary>
        /// Defines the filesInfos.
        /// </summary>
        private IEnumerable<(string key, IFileInfo info)> filesInfos;

        /// <summary>
        /// The GetFileInfos.
        /// </summary>
        /// <returns>The .</returns>
        public IEnumerable<(string key, IFileInfo info)> GetFileInfos()
        {
            if (filesInfos == null)
            {
                // test:/abc/123.txt;/abc/124.txt
                var files = this.Settings.Split(";");
                filesInfos = files.SelectMany(e => ToFileInfos(e));
            }
            return filesInfos;
        }

        /// <summary>
        /// The ToFileInfos.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The.</returns>
        private IEnumerable<(string key, IFileInfo info)> ToFileInfos(string path)
        {
            List<(string key, IFileInfo info)> list = new List<(string key, IFileInfo info)>();
            // abc/cdf/123.txt
            list.Add((path, new InMemoryFileInfo(path, path.GetBytes(), Path.GetFileName(path))));
            var dir = path;
            while (!string.IsNullOrEmpty(dir) && dir.Contains("/"))
            {
                dir = dir.Substring(0, dir.LastIndexOf("/"));
                // /abc/cdf --1
                // /abc --2
                if (dir.Contains("/"))
                {
                    list.Add((dir, new VirtualFileInfo(dir)));
                }
            }
            return list;
        }
    }
}
