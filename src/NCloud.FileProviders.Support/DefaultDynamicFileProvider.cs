// -----------------------------------------------------------------------
// <copyright file="DefaultNCloudDynamicFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Primitives;
    using NCloud.FileProviders.Abstractions;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="DefaultNCloudDynamicFileProvider" />.
    /// </summary>
    public class DefaultNCloudDynamicFileProvider : INCloudDynamicFileProvider
    {
        /// <summary>
        /// Defines the _providers.
        /// </summary>
        private IDictionary<string, BaseNCloudFileProvider> _providers;

        /// <summary>
        /// Defines the _compositeFileProvider.
        /// </summary>
        protected CompositeFileProvider _compositeFileProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultNCloudDynamicFileProvider"/> class.
        /// </summary>
        public DefaultNCloudDynamicFileProvider()
        {
            this._providers = new Dictionary<string, BaseNCloudFileProvider>();
        }

        /// <summary>
        /// The CreateProvider.
        /// </summary>
        /// <param name="providers">The providers<see cref="BaseNCloudFileProvider[]"/>.</param>
        /// <returns>The <see cref="BaseNCloudFileProvider"/>.</returns>
        public bool AddProvider(params BaseNCloudFileProvider[] providers)
        {
            var added = providers.Select((provider) => this._providers.TryAdd(provider.Config, provider)).Any(e => e);
            if (added)
            {
                this._compositeFileProvider = RebuildCompositeProviders();
            }
            return added;
        }

        /// <summary>
        /// The RemoveProvider.
        /// </summary>
        /// <param name="config">The config<see cref="IEnumerable{string}"/>.</param>
        public void RemoveProvider(IEnumerable<string> config)
        {
            var changed = config.Select(e => this._providers.Remove(e)).Any(e => e);
            if (changed)
            {
                this._compositeFileProvider = RebuildCompositeProviders();
            }
        }

        /// <summary>
        /// The GetDirectoryContents.
        /// </summary>
        /// <param name="subpath">The subpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IDirectoryContents"/>.</returns>
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            try
            {
                return _compositeFileProvider.GetDirectoryContents(subpath);
            }
            catch (Exception)
            {
                return NotFoundDirectoryContents.Singleton;
            }
        }

        /// <summary>
        /// The GetFileInfo.
        /// </summary>
        /// <param name="subpath">The subpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        public IFileInfo GetFileInfo(string subpath)
        {
            try
            {
                subpath = subpath.EnsureStartsWith('/');
                if (subpath == "/")
                {
                    return new VirtualFileInfo(subpath);
                }
                return _compositeFileProvider.GetFileInfo(subpath);

            }
            catch (Exception)
            {
                return new NotFoundFileInfo(subpath);
            }
        }

        /// <summary>
        /// The RefreshProviders.
        /// </summary>
        /// <returns>The <see cref="CompositeFileProvider"/>.</returns>
        protected virtual CompositeFileProvider RebuildCompositeProviders()
        {
            return new CompositeFileProvider(this._providers.Values);
        }

        /// <summary>
        /// The Watch.
        /// </summary>
        /// <param name="filter">The filter<see cref="string"/>.</param>
        /// <returns>The <see cref="IChangeToken"/>.</returns>
        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }
    }
}
