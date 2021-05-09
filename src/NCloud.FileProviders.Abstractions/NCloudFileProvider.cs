// -----------------------------------------------------------------------
// <copyright file="NCloudFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Defines the <see cref="NCloudFileProvider" />.
    /// </summary>
    public abstract class NCloudFileProvider : IFileProvider
    {
        /// <summary>
        /// Defines the setting.
        /// </summary>
        protected readonly string setting;

        /// <summary>
        /// Defines the prefix.
        /// </summary>
        protected readonly string prefix;

        /// <summary>
        /// Defines the provider.
        /// </summary>
        protected readonly IServiceProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudFileProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        public NCloudFileProvider(IServiceProvider provider, string config, string prefix)
        {
            this.provider = provider;
            if (!string.IsNullOrEmpty(config))
            {
                var configs = config.Split(":");
                // fs:./example
                this.setting = configs.Length==2 ?configs[1]:string.Empty;
            }
            else
            {
                this.setting = string.Empty;
            }
            this.prefix = prefix;
        }

        /// <summary>
        /// The GetDirectoryContents.
        /// </summary>
        /// <param name="subPath">The subpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IDirectoryContents"/>.</returns>
        public IDirectoryContents GetDirectoryContents(string subPath)
        {
            if (subPath == null || !subPath.StartsWith(prefix))
            {
                return NotFoundDirectoryContents.Singleton;
            }
            var contents = this.GetDirectoryContentsByRelPath(GetRelPath(subPath));
            if (contents == null || !contents.Exists)
            {
                return NotFoundDirectoryContents.Singleton;
            }
            return new EnumerableDirectoryContents(contents.Select(e => new NCloudFileInfo(e, subPath== "/"? "/" + e.Name : subPath + "/" + e.Name)));
        }

        /// <summary>
        /// The GetDirectoryContentsBySubPath.
        /// </summary>
        /// <param name="relpath">The relpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IDirectoryContents"/>.</returns>
        protected abstract IDirectoryContents GetDirectoryContentsByRelPath(string relpath);

        /// <summary>
        /// The GetFileInfo.
        /// </summary>
        /// <param name="subPath">The subpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        public IFileInfo GetFileInfo(string subPath)
        {
            if (subPath == null || !subPath.StartsWith(prefix))
            {
                return new NotFoundFileInfo(subPath);
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
        /// NCloud did not support Watch.
        /// </summary>
        /// <param name="filter">The filter<see cref="string"/>.</param>
        /// <returns>The <see cref="IChangeToken"/>.</returns>
        public virtual IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        /// <summary>
        /// The GetRelPath.
        /// </summary>
        /// <param name="subPath">The subPath<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        protected string GetRelPath(string subPath)
        {
            var relative = subPath.Substring(prefix.Length);
            if (relative.StartsWith("/"))
            {
                relative = relative.Substring(1);
            }
            return relative;
        }
    }
}
