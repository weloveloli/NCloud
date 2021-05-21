// -----------------------------------------------------------------------
// <copyright file="CompositeNCloudFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Logging;
    using NCloud.FileProviders.Abstractions;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="EmbeddableCompositeNCloudFileProvider" />.
    /// </summary>
    public abstract class EmbeddableCompositeNCloudFileProvider : BaseNCloudFileProvider, INCloudFileProviderRegistry
    {
        /// <summary>
        /// Defines the factory.
        /// </summary>
        protected INCloudFileProviderFactory factory;

        /// <summary>
        /// Defines the _providers.
        /// </summary>
        private IDictionary<string, BaseNCloudFileProvider> _providers;

        /// <summary>
        /// Defines the _compositeFileProvider.
        /// </summary>
        protected CompositeFileProvider _compositeFileProvider;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private ILogger<EmbeddableCompositeNCloudFileProvider> logger;

        /// <summary>
        /// Defines the lazyLoaded.
        /// </summary>
        protected bool lazyload;

        /// <summary>
        /// Defines the loadedPrefixs.
        /// </summary>
        protected HashSet<string> loadedPrefixs;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddableCompositeNCloudFileProvider"/> class.
        /// </summary>
        /// <param name="lazyLoaded">The lazyLoaded<see cref="bool"/>.</param>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        public EmbeddableCompositeNCloudFileProvider(bool lazyLoaded, IServiceProvider serviceProvider, string config, string prefix) : base(serviceProvider, config, prefix)
        {
            this.lazyload = lazyLoaded;
            this.logger = serviceProvider.GetService<ILogger<EmbeddableCompositeNCloudFileProvider>>();
            this.factory = serviceProvider.GetService<INCloudFileProviderFactory>();
            this._providers = new Dictionary<string, BaseNCloudFileProvider>();
            this._compositeFileProvider = RebuildCompositeProviders();
            this.loadedPrefixs = new HashSet<string>();
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
        public override IDirectoryContents GetDirectoryContents(string subpath)
        {
            try
            {
                var sub = _compositeFileProvider.GetDirectoryContents(subpath);
                if (lazyload)
                {
                    TryResolveEmbedded(sub);
                }
                return sub;
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetDirectoryContents error, subpath is {subpath}", subpath);
                return NotFoundDirectoryContents.Singleton;
            }
        }

        /// <summary>
        /// The ResolveEmbed.
        /// </summary>
        /// <param name="sub">The sub<see cref="IEnumerable{IFileInfo}"/>.</param>
        protected bool TryResolveEmbedded(IEnumerable<IFileInfo> sub)
        {
            if (sub.Any(e => e is EmbeddedFileInfo))
            {
                var embeded = sub
                .Where(e => e is EmbeddedFileInfo)
                .Select(e => (EmbeddedFileInfo)e)
                .Where(e => !loadedPrefixs.Contains(e.Prefix));
                if (!embeded.Any())
                {
                    return false;
                }
                var providers = new List<BaseNCloudFileProvider>();
                foreach (var embed in embeded)
                {
                    var config = embed.GetProviderConfig();
                    try
                    {
                        var provider = factory.CreateProvider(config, embed.Prefix);
                        if (provider != null)
                        {
                            providers.Add(provider);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "load embeded provider failed");
                    }
                    loadedPrefixs.Add(embed.Prefix);
                }

                this.AddProvider(providers.ToArray());
                return true;
            }
            return false;
        }

        /// <summary>
        /// The GetFileInfo.
        /// </summary>
        /// <param name="subpath">The subpath<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileInfo"/>.</returns>
        public override IFileInfo GetFileInfo(string subpath)
        {
            try
            {
                subpath = subpath.EnsureStartsWith('/');

                return _compositeFileProvider.GetFileInfo(subpath);

            }
            catch (Exception e)
            {
                logger.LogError(e, "GetFileInfo error, subpath is {subpath}", subpath);
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
    }
}
