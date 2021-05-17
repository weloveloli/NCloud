// -----------------------------------------------------------------------
// <copyright file="BaseNCloudFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using System;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Primitives;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="BaseNCloudFileProvider" />.
    /// </summary>
    public abstract class BaseNCloudFileProvider : IFileProvider
    {
        /// <summary>
        /// Gets the Config
        /// Gets or sets the Config........
        /// </summary>
        public string Config => this.config;

        /// <summary>
        /// Gets the Prefix
        /// Gets or sets the Protocal........
        /// </summary>
        public string Prefix => this.prefix;

        /// <summary>
        /// Defines the config.
        /// </summary>
        protected readonly string config;

        /// <summary>
        /// Defines the prefix.
        /// </summary>
        protected readonly string prefix;

        /// <summary>
        /// Defines the setting.
        /// </summary>
        protected readonly string setting;

        /// <summary>
        /// Defines the provider.
        /// </summary>
        protected readonly IServiceProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseNCloudFileProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        protected BaseNCloudFileProvider(IServiceProvider provider, string config, string prefix)
        {
            this.provider = provider;
            this.config = config;
            this.prefix = prefix.EnsureStartsWith('/');
            if (!string.IsNullOrEmpty(config))
            {
                var configs = config.Split(":");
                // fs:./example
                this.setting = configs.Length == 2 ? configs[1] : string.Empty;
            }
            else
            {
                this.setting = string.Empty;
            }
        }

        /// <summary>
        /// The GetFileInfo.
        /// </summary>
        /// <param name="subpath">The subpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        public abstract IFileInfo GetFileInfo(string subpath);

        /// <summary>
        /// The GetDirectoryContents.
        /// </summary>
        /// <param name="subpath">The subpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IDirectoryContents"/>.</returns>
        public abstract IDirectoryContents GetDirectoryContents(string subpath);

        /// <summary>
        /// NCloud did not support Watch.
        /// </summary>
        /// <param name="filter">The filter<see cref="string"/>.</param>
        /// <returns>The <see cref="IChangeToken"/>.</returns>
        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }
    }
}
