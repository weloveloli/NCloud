// -----------------------------------------------------------------------
// <copyright file="BaseDrive.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="BaseDrive" />.
    /// </summary>
    public abstract class BaseDrive : IDrive
    {
        /// <summary>
        /// Defines the config.
        /// </summary>
        protected readonly string config;

        /// <summary>
        /// Defines the serviceProvider.
        /// </summary>
        protected readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Defines the pathFromRoot.
        /// </summary>
        protected readonly string pathFromRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDrive"/> class.
        /// </summary>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="pathFromRoot">The pathFromRoot<see cref="string"/>.</param>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        public BaseDrive(string config, string pathFromRoot, IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(config))
            {
                throw new ArgumentException($"'{nameof(config)}' cannot be null or whitespace.", nameof(config));
            }
            this.config = config;
            this.serviceProvider = serviceProvider;
            this.pathFromRoot = pathFromRoot;
        }

        /// <summary>
        /// The GetConfig.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetConfig()
        {
            return config;
        }

        /// <summary>
        /// The GetFileInfosByPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="List{FileInfo}"/>.</returns>
        public abstract Task<NCloudResult> GetFileInfosByPathAsync(string path);

        /// <summary>
        /// The GetFileStreamByPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Task<Stream> GetFileStreamByPathAsync(string path, bool cache = true)
        {
            if (this is IContentDrive drive)
            {
                return drive.GetFileStream(path, cache);
            }
            return null;
        }

        /// <summary>
        /// The GetInnerConfig.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        protected string GetSetting()
        {
            return config.Substring(config.IndexOf(":") + 1);
        }
    }
}
