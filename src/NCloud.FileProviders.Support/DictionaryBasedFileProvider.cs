// -----------------------------------------------------------------------
// <copyright file="DictionaryBasedFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="DictionaryBasedFileProvider" />.
    /// </summary>
    public class DictionaryBasedFileProvider : PrefixNCloudFileProvider
    {
        /// <summary>
        /// Defines the _files.
        /// </summary>
        protected IDictionary<string, IFileInfo> _files;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryBasedFileProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        public DictionaryBasedFileProvider(IServiceProvider provider, string config, string prefix) : base(provider, config, prefix)
        {
            _files = new Dictionary<string, IFileInfo>();
        }

        /// <summary>
        /// Gets the Files.
        /// </summary>
        protected virtual IDictionary<string, IFileInfo> Files => _files;

        /// <summary>
        /// The Rebuild.
        /// </summary>
        /// <param name="fileInfos">The fileInfos<see cref="IEnumerable{(string, IFileInfo)}"/>.</param>
        public void Rebuild(IEnumerable<(string key, IFileInfo info)> fileInfos)
        {
            var items = fileInfos.Distinct(new LambdaEqual<(string key, IFileInfo info)>((e) => e.key));
            _files = items.ToDictionary(t => t.key, t => t.info);
        }

        /// <summary>
        /// The GetFileInfo.
        /// </summary>
        /// <param name="relpath">The relpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        protected override IFileInfo GetFileInfoByRelPath(string relpath)
        {
            if (relpath == null)
            {
                return new NotFoundFileInfo(relpath);
            }
            relpath = relpath.EnsureStartsWith('/');
            if (relpath == "/")
            {
                return new VirtualFileInfo(relpath);
            }

            return Files.GetOrDefault(NormalizePath(relpath))?? new NotFoundFileInfo(relpath);
        }

        /// <summary>
        /// The GetDirectoryContents.
        /// </summary>
        /// <param name="relpath">The relpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IDirectoryContents"/>.</returns>
        protected override IDirectoryContents GetDirectoryContentsByRelPath(string relpath)
        {
            var directoryPath = relpath.EnsureEndsWith('/').EnsureStartsWith('/');

            var directory = GetFileInfoByRelPath(relpath.EnsureStartsWith('/'));
            if (!directory.IsDirectory)
            {
                return NotFoundDirectoryContents.Singleton;
            }

            var fileList = new List<IFileInfo>();
            foreach (var fileInfo in Files.Values)
            {
                var fullPath = fileInfo.GetVirtualOrPhysicalPath();
                if (fullPath is null || !fullPath.StartsWith(directoryPath))
                {
                    continue;
                }

                var relativePath = fullPath.Substring(directoryPath.Length);
                if (relativePath.Contains("/"))
                {
                    continue;
                }

                fileList.Add(fileInfo);
            }

            return new EnumerableDirectoryContents(fileList);
        }

        /// <summary>
        /// The NormalizePath.
        /// </summary>
        /// <param name="subPath">The subPath<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        protected virtual string NormalizePath(string subPath)
        {
            return subPath;
        }
    }
}
