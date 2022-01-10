// -----------------------------------------------------------------------
// <copyright file="NCloudStaticServerOptions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.StaticServer.Configuration
{
    using System;
    using System.IO;
    using NCloud.EndPoints.FTP.Configurations;
    using NCloud.EndPoints.WebDAV.Configurations;
    using NCloud.FileProviders.Abstractions;

    /// <summary>
    /// Defines the <see cref="NCloudStaticServerOptions" />.
    /// </summary>
    public class NCloudStaticServerOptions : ISystemConfigProvider
    {
        /// <summary>
        /// Gets or sets the Ftp.
        /// </summary>
        public FtpOptions Ftp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ftp endpoint is Enable......
        /// </summary>
        public bool FtpEnable { get; set; }

        /// <summary>
        /// Gets or sets the WebDAVConfig.
        /// </summary>
        public WebDAVConfig WebDAVConfig { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether WebDAVEnable.
        /// </summary>
        public bool WebDAVEnable { get; set; }

        /// <summary>
        /// Defines the _configFolder.
        /// </summary>
        private string _configFolder;

        /// <summary>
        /// Defines the _cacheFolder.
        /// </summary>
        private string _cacheFolder;

        /// <summary>
        /// Gets or sets the ConfigFolder.
        /// </summary>
        public string ConfigFolder {
            get {
                if (!string.IsNullOrEmpty(_configFolder))
                {
                    return _configFolder;
                }
                _configFolder = Path.Combine(Environment.CurrentDirectory, "config");
                if (!Directory.Exists(_configFolder))
                {
                    Directory.CreateDirectory(_configFolder);
                }
                return _configFolder;
            }
            set {
                _configFolder = value;
            }
        }

        /// <summary>
        /// Gets or sets the CacheFolder.
        /// </summary>
        public string CacheFolder {
            get {
                if (!string.IsNullOrEmpty(_cacheFolder))
                {
                    return _cacheFolder;
                }
                _cacheFolder = Path.Combine(Environment.CurrentDirectory, "cache");
                if (!Directory.Exists(_cacheFolder))
                {
                    Directory.CreateDirectory(_cacheFolder);
                }
                return _cacheFolder;
            }
            set {
                _cacheFolder = value;
            }
        }
    }
}
