// -----------------------------------------------------------------------
// <copyright file="NCloudPhysicalFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Physical
{
    using System;
    using System.IO;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.Support;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="PhysicalNCloudFileProvider" />.
    /// </summary>
    [FileProvider(Name = "fs", Protocol = "fs")]
    public class PhysicalNCloudFileProvider : PrefixNCloudFileProvider
    {
        /// <summary>
        /// Defines the _provider.
        /// </summary>
        private readonly PhysicalFileProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalNCloudFileProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        public PhysicalNCloudFileProvider(IServiceProvider provider, string config, string prefix) : base(provider, config, prefix)
        {
            var realPath = setting.FromPosixPath();
            if (!Directory.Exists(realPath))
            {
                throw new ArgumentException($"{realPath} not exist");
            }
            var di = new DirectoryInfo(realPath);
            this._provider = new PhysicalFileProvider(di.FullName);
        }

        /// <summary>
        /// The GetDirectoryContentsByRelPath.
        /// </summary>
        /// <param name="relpath">The relpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IDirectoryContents"/>.</returns>
        protected override IDirectoryContents GetDirectoryContentsByRelPath(string relpath)
        {
            return this._provider.GetDirectoryContents(relpath);
        }

        /// <summary>
        /// The GetFileInfoByRelPath.
        /// </summary>
        /// <param name="relPath">The relPath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        protected override IFileInfo GetFileInfoByRelPath(string relPath)
        {
            return this._provider.GetFileInfo(relPath);
        }
    }
}
