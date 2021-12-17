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

    /// <summary>
    /// Defines the <see cref="BaseNCloudFileProvider{IProviderConfigType}" />.
    /// </summary>
    /// <typeparam name="IProviderConfigType">.</typeparam>
    public abstract class BaseNCloudFileProvider<IProviderConfigType> : INCloudFileProvider
        where IProviderConfigType : BaseProviderConfig, new()
    {
        /// <summary>
        /// Gets the Config.
        /// </summary>
        public IProviderConfigType Config => this.config;

        /// <summary>
        /// Gets the Prefix.
        /// </summary>
        public string Prefix => config.Prefix;

        /// <summary>
        /// Defines the config.
        /// </summary>
        protected readonly IProviderConfigType config;

        /// <summary>
        /// Defines the provider.
        /// </summary>
        protected readonly IServiceProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseNCloudFileProvider{IProviderConfigType}"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        protected BaseNCloudFileProvider(IServiceProvider provider, IProviderConfigType config)
        {
            this.provider = provider;
            this.config = config;
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

        /// <summary>
        /// Gets the Key
        /// The Key..
        /// </summary>
        public string Key => this.config.HashKey();
    }
}
