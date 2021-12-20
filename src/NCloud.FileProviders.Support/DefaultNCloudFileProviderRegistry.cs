// -----------------------------------------------------------------------
// <copyright file="DefaultNCloudFileProviderRegistry.cs" company="Weloveloli">
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
    /// Defines the <see cref="DefaultNCloudFileProviderRegistry" />.
    /// </summary>
    public class DefaultNCloudFileProviderRegistry : INCloudFileProviderRegistry
    {
        /// <summary>
        /// Defines the _providers.
        /// </summary>
        private IDictionary<string, INCloudFileProvider> _providers;

        /// <summary>
        /// Defines the _compositeFileProvider.
        /// </summary>
        protected CompositeFileProvider _compositeFileProvider;

        /// <summary>
        /// Gets the Key.
        /// </summary>
        public string Key => "DefaultNCloudFileProviderRegistry";

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultNCloudFileProviderRegistry"/> class.
        /// </summary>
        public DefaultNCloudFileProviderRegistry()
        {
            this._providers = new Dictionary<string, INCloudFileProvider>();
        }

        /// <summary>
        /// The CreateProvider.
        /// </summary>
        /// <param name="providers">The providers.</param>
        /// <returns>The <see cref="INCloudFileProvider"/>.</returns>
        public bool AddProvider(params INCloudFileProvider[] providers)
        {
            var added = false;
            foreach (var provider in providers)
            {
                if (provider != null)
                {
                    added = this._providers.TryAdd(provider.Key, provider) || added;
                }

            }
            if (added)
            {
                this._compositeFileProvider = RebuildCompositeProviders();
            }
            return added;
        }

        /// <summary>
        /// The RemoveProvider.
        /// </summary>
        /// <param name="keys">The keys<see cref="IEnumerable{string}"/>.</param>
        public void RemoveProvider(IEnumerable<string> keys)
        {
            var changed = keys.Select(e => this._providers.Remove(e)).Any(e => e);
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
