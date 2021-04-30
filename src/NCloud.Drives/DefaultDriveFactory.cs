// -----------------------------------------------------------------------
// <copyright file="DriveFactory.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Drives
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NCloud.Core;
    using NCloud.Core.Attributes;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="DefaultDriveFactory" />.
    /// </summary>
    public class DefaultDriveFactory : IDriveFactory
    {
        /// <summary>
        /// Defines the drives.
        /// </summary>
        private readonly IDictionary<string, IDrive> drives;

        /// <summary>
        /// Defines the roots.
        /// </summary>
        private readonly IDictionary<string, string> roots;

        /// <summary>
        /// Defines the provider.
        /// </summary>
        private readonly IServiceProvider provider;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<DefaultDriveFactory> logger;

        /// <summary>
        /// Defines the drivesTypes.
        /// </summary>
        private readonly Dictionary<string, Type> drivesTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDriveFactory"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        public DefaultDriveFactory(IServiceProvider provider)
        {
            drives = new Dictionary<string, IDrive>();
            roots = new Dictionary<string, string>();
            this.provider = provider;
            this.logger = (ILogger<DefaultDriveFactory>)provider.GetService(typeof(ILogger<DefaultDriveFactory>));
            this.drivesTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(e => e.IsSubclassOf(typeof(BaseDrive)) && e.GetCustomAttributes(typeof(DriveAttribute), false).Length == 1)
                .Select(e => (((DriveAttribute)e.GetCustomAttributes(typeof(DriveAttribute), false)[0]).Protocol, e))
                .ToDictionary(e => e.Protocol, e => e.e);
        }

        /// <summary>
        /// The GetFileInfosByPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="List{FileInfo}"/>.</returns>
        public async Task<NCloudResult> GetFileInfosByPathAsync(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path) || path == "/")
                {
                    var list = roots.Select(e => new Core.Model.FileInfo
                    {
                        Size = 0,
                        Name = e.Key,
                        Path = e.Value,
                        Type = "dir"
                    });
                    return NCloudResult.OK(list);
                }
                var drive = FindNeareastDrive(path, out var relative);
                return await drive.GetFileInfosByPathAsync(relative);
            }
            catch (Exception e)
            {
                return NCloudResult.Error(e);
            }
        }

        /// <summary>
        /// The FindNeareastNode.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="relative">The relative<see cref="string"/>.</param>
        /// <returns>The <see cref="PathTreeNode"/>.</returns>
        private IDrive FindNeareastDrive(string path, out string relative)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }
            var pair = drives.Where(e => path.StartsWith(e.Key)).OrderByDescending(e => e.Key.Length);
            if (!pair.Any())
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }
            var drivePath = pair.First().Key;
            relative = path.Substring(drivePath.Length);
            if (relative.StartsWith("/"))
            {
                relative = relative.Substring(1);
            }
            return pair.First().Value;
        }

        /// <summary>
        /// The GetDrive.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="force">The force<see cref="bool"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool TryEnableDrive(string path, string config, bool force = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
                }
                if (drives.ContainsKey(path))
                {
                    if (drives[path].GetConfig() == config)
                    {
                        return true;
                    }
                    else
                    {
                        drives.Remove(path);
                    }
                }
                var type = GetDriveType(config);
                var drive = (IDrive)Activator.CreateInstance(type, new object[] { config, path, provider });
                drives.Add(path, drive);
                return true;
            }
            catch (Exception)
            {
                return false;
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
            var driveType = drivesTypes.GetValueOrDefault(protocol) ?? throw new ArgumentException($"'{nameof(config)}' is invalid, {protocol} is not support.", nameof(config));
            if (!driveType.IsSubclassOf(typeof(BaseDrive)))
            {
                throw new ArgumentException($"'{nameof(driveType)}' is invalid, must be subclass of BaseDrive.", nameof(driveType));
            }
            return driveType;
        }

        /// <summary>
        /// The GetConfig.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetConfig()
        {
            return string.Empty;
        }

        /// <summary>
        /// The GetFileStream.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="cache">The cache<see cref="bool"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Task<Stream> GetFileStreamByPathAsync(string path, bool cache = true)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }
            var drive = FindNeareastDrive(path, out var relative);
            return drive.GetFileStreamByPathAsync(relative, cache);
        }

        /// <summary>
        /// The TryAddRoot.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="force">The force<see cref="bool"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool TryAddRoot(string path, string name, bool force = true)
        {
            if (roots.ContainsKey(name))
            {
                if (!force)
                {
                    return false;
                }
                else
                {
                    roots.Remove(name);
                }
            }
            roots.Add(name, path);
            return true;
        }
    }
}
