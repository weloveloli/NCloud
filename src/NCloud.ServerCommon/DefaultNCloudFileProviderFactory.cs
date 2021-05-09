// -----------------------------------------------------------------------
// <copyright file="DefaultNCloudFileProviderFactory.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.ServerCommon
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.Abstractions.Extensions;

    /// <summary>
    /// Defines the <see cref="DefaultNCloudFileProviderFactory" />.
    /// </summary>
    public class DefaultNCloudFileProviderFactory : INCloudFileProviderFactory, IFileProvider
    {
        /// <summary>
        /// Defines the serviceProvider.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Defines the _providers.
        /// </summary>
        private IDictionary<string, NCloudFileProvider> _providers;

        /// <summary>
        /// Defines the _providerTypes.
        /// </summary>
        private IDictionary<string, Type> _providerTypes;

        /// <summary>
        /// Defines the _compositeFileProvider.
        /// </summary>
        private CompositeFileProvider _compositeFileProvider;

        /// <summary>
        /// Defines the prefixs.
        /// </summary>
        private IDictionary<string, string> prefixs;

        /// <summary>
        /// Defines the dictProvider.
        /// </summary>
        private DictionaryBasedFileProvider dictProvider;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private ILogger<DefaultNCloudFileProviderFactory> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultNCloudFileProviderFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        public DefaultNCloudFileProviderFactory(IServiceProvider serviceProvider)
        {
            this.logger = serviceProvider.GetService<ILogger<DefaultNCloudFileProviderFactory>>();
            this._providers = new Dictionary<string, NCloudFileProvider>();
            this.prefixs = new Dictionary<string, string>();
            List<Assembly> fileProviderAssemblies = new List<Assembly>();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (string dll in Directory.GetFiles(path, "NCloud.FileProviders.*.dll"))
            {
                fileProviderAssemblies.Add(Assembly.LoadFile(dll));
            }
            var types = fileProviderAssemblies.SelectMany(e => e.GetExportedTypes());
            this._providerTypes = types
                .Where(e => e.IsSubclassOf(typeof(NCloudFileProvider)))
                .Where(e => e.GetCustomAttributes(typeof(FileProviderAttribute), false).Length == 1)
                .Select(e => (((FileProviderAttribute)e.GetCustomAttributes(typeof(FileProviderAttribute), false)[0]).Protocol, e))
                .ToDictionary(e => e.Protocol, e => e.e);
            this.serviceProvider = serviceProvider;
            RefreshProviders();
        }

        /// <summary>
        /// The CreateProvider.
        /// </summary>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        /// <returns>The <see cref="NCloudFileProvider"/>.</returns>
        public NCloudFileProvider CreateProvider(string config, string prefix = "")
        {
            NCloudFileProvider provider = this._providers.GetOrDefault(config);
            if (provider != null)
            {
                return provider;
            }
            var type = GetDriveType(config);
            provider = (NCloudFileProvider)Activator.CreateInstance(type, new object[] { serviceProvider, config, prefix });
            this._providers.Add(config, provider);
            this.prefixs.Add(config, prefix);
            RefreshProviders();
            return provider;
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
                subpath = subpath.EnsureStartsWith('/');
                if (subpath == "/")
                {
                    return dictProvider.GetDirectoryContents(subpath);
                }
                else
                {
                    return _compositeFileProvider.GetDirectoryContents(subpath);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetDirectoryContents error, subpath is {subpath}", subpath);
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
                    return dictProvider.GetFileInfo(subpath);
                }
                else
                {
                    return _compositeFileProvider.GetFileInfo(subpath);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetFileInfo error, subpath is {subpath}", subpath);
                return new NotFoundFileInfo(subpath);
            }
        }

        /// <summary>
        /// The RemoveProvider.
        /// </summary>
        /// <param name="config">The config<see cref="IEnumerable{string}"/>.</param>
        public void RemoveProvider(IEnumerable<string> config)
        {
            var changed = config.Select(e => this._providers.Remove(e)).Where(e => e).Any();
            _ = config.Select(e => this.prefixs.Remove(e));
            if (changed)
            {
                RefreshProviders();
            }
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

        /// <summary>
        /// The GetDriveType.
        /// </summary>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        private Type GetDriveType(string config)
        {
            if (string.IsNullOrWhiteSpace(config))
            {
                throw new ArgumentException($"'{nameof(config)}' cannot be null or whitespace.", nameof(config));
            }
            if (!config.Contains(":"))
            {
                throw new ArgumentException($"'{nameof(config)}' is invalid.", nameof(config));
            }
            var protocol = config.Substring(0, config.IndexOf(":"));
            var providerType = _providerTypes.GetOrDefault(protocol) ?? throw new ArgumentException($"'{nameof(config)}' is invalid, {protocol} is not support.", nameof(config));
            if (!providerType.IsSubclassOf(typeof(NCloudFileProvider)))
            {
                throw new ArgumentException($"'{nameof(providerType)}' is invalid, must be subclass of NCloudFileProvider.", nameof(providerType));
            }
            return providerType;
        }

        /// <summary>
        /// The ToFileInfos.
        /// </summary>
        /// <param name="prefix">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="IEnumerable{(string key, IFileInfo info)}"/>.</returns>
        private IEnumerable<(string key, IFileInfo info)> ToFileInfos(string prefix)
        {
            List<(string key, IFileInfo info)> list = new();
            list.Add((prefix, new VirtualFileInfo(prefix)));
            var dir = prefix;
            while (!string.IsNullOrEmpty(dir) && dir.Contains("/"))
            {
                dir = dir.Substring(0, dir.LastIndexOf("/"));
                if (dir.Contains("/"))
                {
                    list.Add((dir, new VirtualFileInfo(dir)));
                }
            }
            return list;
        }

        /// <summary>
        /// The RefreshProviders.
        /// </summary>
        private void RefreshProviders()
        {
            dictProvider ??= new DictionaryBasedFileProvider(serviceProvider, null, "/");
            dictProvider.Rebuild(this.prefixs.Values.SelectMany(e => ToFileInfos(e)));
            _compositeFileProvider = new CompositeFileProvider(this._providers.Values);
        }
    }
}
