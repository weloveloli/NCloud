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
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
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
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<DefaultNCloudFileProviderFactory> logger;

        /// <summary>
        /// Defines the _providers.
        /// </summary>
        private IDictionary<string, INCloudFileProvider> _providers;

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
            this.logger = serviceProvider.GetService<ILogger<DefaultNCloudFileProviderFactory>>();
            this._providers = new Dictionary<string, INCloudFileProvider>();
            this._prefixs = new Dictionary<string, string>();
            var fileProviderAssemblies = new List<Assembly>();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (string dll in Directory.GetFiles(path, "NCloud.FileProviders.*.dll"))
            {
                fileProviderAssemblies.Add(Assembly.LoadFrom(dll));
            }
            var types = fileProviderAssemblies.SelectMany(e => e.GetExportedTypes())
                .Where(e => e.IsSubclassOf(typeof(INCloudFileProvider)));
            this._providerTypes = types
                .Where(e => e.GetCustomAttributes(typeof(FileProviderAttribute), false).Length == 1)
                .Select(e => (((FileProviderAttribute)e.GetCustomAttributes(typeof(FileProviderAttribute), false)[0]).Type, e))
                .ToDictionary(e => e.Type, e => e.e);
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// The CreateProvider.
        /// </summary>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <returns>The <see cref="PrefixNCloudFileProvider"/>.</returns>
        public INCloudFileProvider CreateProvider(BaseProviderConfig config)
        {
            var key = config.HashKey();
            var provider = this._providers.GetOrDefault(key);
            if (provider != null)
            {
                return provider;
            }
            var type = GetDriveType(config.Type);
            try
            {
                provider = (INCloudFileProvider)Activator.CreateInstance(type, new object[] { serviceProvider, config });
                this._providers.Add(key, provider);
                this._prefixs.Add(key, config.Prefix);
                return provider;
            }
            catch (Exception e)
            {
                this.logger?.LogError("CreateProvider Failed", e);
                return null;
            }
        }

        /// <summary>
        /// The GetDriveType.
        /// </summary>
        /// <param name="type">The config<see cref="string"/>.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        private Type GetDriveType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException($"'{nameof(type)}' cannot be null or whitespace.", nameof(type));
            }
            var providerType = _providerTypes.GetOrDefault(type) ?? throw new ArgumentException($"'{nameof(type)}' is invalid, {type} is not support.", nameof(type));
            if (!providerType.IsSubclassOf(typeof(INCloudFileProvider)))
            {
                throw new ArgumentException($"'{nameof(providerType)}' {providerType} is invalid, must be subclass of INCloudFileProvider.", nameof(providerType));
            }
            return providerType;
        }
    }
}
