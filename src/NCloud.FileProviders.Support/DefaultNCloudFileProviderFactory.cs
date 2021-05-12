// -----------------------------------------------------------------------
// <copyright file="DefaultNCloudFileProviderFactory.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using NCloud.FileProviders.Abstractions;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="DefaultNCloudFileProviderFactory" />.
    /// </summary>
    public class DefaultNCloudFileProviderFactory : INCloudFileProviderFactory
    {
        /// <summary>
        /// Defines the serviceProvider.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Defines the _providers.
        /// </summary>
        private IDictionary<string, BaseNCloudFileProvider> _providers;

        /// <summary>
        /// Defines the _providerTypes.
        /// </summary>
        private IDictionary<string, Type> _providerTypes;

        /// <summary>
        /// Defines the _prefixs.
        /// </summary>
        private IDictionary<string, string> _prefixs;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultNCloudFileProviderFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        public DefaultNCloudFileProviderFactory(IServiceProvider serviceProvider)
        {
            this._providers = new Dictionary<string, BaseNCloudFileProvider>();
            this._prefixs = new Dictionary<string, string>();
            var fileProviderAssemblies = new List<Assembly>();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (string dll in Directory.GetFiles(path, "NCloud.FileProviders.*.dll"))
            {
                fileProviderAssemblies.Add(Assembly.LoadFile(dll));
            }
            var types = fileProviderAssemblies.SelectMany(e => e.GetExportedTypes());
            this._providerTypes = types
                .Where(e => e.IsSubclassOf(typeof(BaseNCloudFileProvider)))
                .Where(e => e.GetCustomAttributes(typeof(FileProviderAttribute), false).Length == 1)
                .Select(e => (((FileProviderAttribute)e.GetCustomAttributes(typeof(FileProviderAttribute), false)[0]).Protocol, e))
                .ToDictionary(e => e.Protocol, e => e.e);
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// The CreateProvider.
        /// </summary>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        /// <returns>The <see cref="PrefixNCloudFileProvider"/>.</returns>
        public BaseNCloudFileProvider CreateProvider(string config, string prefix = "")
        {
            BaseNCloudFileProvider provider = this._providers.GetOrDefault(config);
            if (provider != null)
            {
                return provider;
            }
            var type = GetDriveType(config);
            try
            {
                provider = (BaseNCloudFileProvider)Activator.CreateInstance(type, new object[] { serviceProvider, config, prefix });
                this._providers.Add(config, provider);
                this._prefixs.Add(config, prefix);
                return provider;
            }
            catch (Exception)
            {
                return null;
            }
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
            if (!providerType.IsSubclassOf(typeof(PrefixNCloudFileProvider)))
            {
                throw new ArgumentException($"'{nameof(providerType)}' is invalid, must be subclass of NCloudFileProvider.", nameof(providerType));
            }
            return providerType;
        }
    }
}
