// -----------------------------------------------------------------------
// <copyright file="NCloudFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="PrefixNCloudFileProvider" />.
    /// </summary>
    public abstract class PrefixNCloudFileProvider<IProviderConfigType> : BaseNCloudFileProvider<IProviderConfigType>
        where IProviderConfigType : BaseProviderConfig, new()
    {
        /// <summary>
        /// Defines the rootFileInfo.
        /// </summary>
        private readonly VirtualFileInfo rootFileInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixNCloudFileProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        public PrefixNCloudFileProvider(IServiceProvider provider, IProviderConfigType config) : base(provider, config)
        {
            this.rootFileInfo = new VirtualFileInfo(Prefix);
        }

        /// <summary>
        /// The GetDirectoryContents.
        /// </summary>
        /// <param name="subPath">The subpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IDirectoryContents"/>.</returns>
        public override IDirectoryContents GetDirectoryContents(string subPath)
        {
            if (subPath == null)
            {
                return NotFoundDirectoryContents.Singleton;
            }
            subPath = subPath.EnsureStartsWith('/');
            if (Prefix.IsSubpathOf(subPath))
            {
                return new EnumerableDirectoryContents(rootFileInfo);
            }
            if (!subPath.StartsWith(Prefix))
            {
                return NotFoundDirectoryContents.Singleton;
            }
            var contents = this.GetDirectoryContentsByRelPath(GetRelPath(subPath));
            if (contents == null || !contents.Exists)
            {
                return NotFoundDirectoryContents.Singleton;
            }
            return new EnumerableDirectoryContents(contents.Select(e => new NCloudFileInfo(e, subPath == "/" ? "/" + e.Name : subPath + "/" + e.Name)));
        }

        /// <summary>
        /// The GetDirectoryContentsBySubPath.
        /// </summary>
        /// <param name="relPath">The relpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IDirectoryContents"/>.</returns>
        protected abstract IDirectoryContents GetDirectoryContentsByRelPath(string relPath);

        /// <summary>
        /// The GetFileInfo.
        /// </summary>
        /// <param name="subPath">The subpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        public override IFileInfo GetFileInfo(string subPath)
        {
            if (subPath == null || !subPath.StartsWith(Prefix))
            {
                return new NotFoundFileInfo(subPath);
            }
            subPath = subPath.EnsureStartsWith('/');
            if (subPath == Prefix)
            {
                return new VirtualFileInfo(subPath);
            }
            try
            {
                var fileInfo = this.GetFileInfoByRelPath(GetRelPath(subPath));
                if (fileInfo == null || !fileInfo.Exists)
                {
                    return new NotFoundFileInfo(subPath);
                }
                else
                {
                    return new NCloudFileInfo(fileInfo, subPath);
                }
            }
            catch
            {
                return new NotFoundFileInfo(subPath);
            }
        }

        /// <summary>
        /// The GetFileInfoBySubPath.
        /// </summary>
        /// <param name="relPath">The relPath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        protected abstract IFileInfo GetFileInfoByRelPath(string relPath);

        /// <summary>
        /// The GetRelPath.
        /// </summary>
        /// <param name="subPath">The subPath<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        protected string GetRelPath(string subPath)
        {
            var relative = subPath.Substring(Prefix.Length);
            if (relative.StartsWith("/"))
            {
                relative = relative.Substring(1);
            }
            return relative;
        }
    }
}
