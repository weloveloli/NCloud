// -----------------------------------------------------------------------
// <copyright file="TestFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AbstractionsTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.Abstractions.Extensions;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="TestFileProvider" />.
    /// </summary>
    [FileProvider(Name = "test", Protocol = "test")]

    public class TestFileProvider : DictionaryBasedFileProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestFileProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        public TestFileProvider(IServiceProvider provider, string config, string prefix) : base(provider, config, prefix)
        {
            // test:/abc/123.txt;/abc/124.txt
            var files = this.setting.Split(";");
            var items = files.SelectMany(e => ToFileInfos(e));
            this.Rebuild(items);
        }
        /// <summary>
        /// The ToFileInfos.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="IEnumerable{(string key, IFileInfo info)}"/>.</returns>
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
