// -----------------------------------------------------------------------
// <copyright file="LocalDrive.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Drives
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using NCloud.Core;
    using NCloud.Core.Attributes;
    using NCloud.Core.Model;
    using FileInfo = Core.Model.FileInfo;

    /// <summary>
    /// Defines the <see cref="LocalDrive" />.
    /// </summary>
    [Drive(Name = "Local", Protocol = "fs")]
    public class LocalDrive : BaseDrive
    {
        /// <summary>
        /// Defines the realPath.
        /// </summary>
        private readonly string realPath;

        /// <summary>
        /// Defines the helper.
        /// </summary>
        private readonly ISystemHelper helper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDrive"/> class.
        /// </summary>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="pathFromRoot">The pathFromRoot<see cref="string"/>.</param>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        public LocalDrive(string config, string pathFromRoot, IServiceProvider serviceProvider) : base(config, pathFromRoot, serviceProvider)
        {
            helper = (ISystemHelper)serviceProvider.GetService(typeof(ISystemHelper));
            var setting = GetSetting();
            realPath = helper.DenormalizePath(setting);
            if (!Directory.Exists(realPath))
            {
                throw new ArgumentException($"{realPath} not exist");
            }
        }

        /// <summary>
        /// The GetFileInfosByPathAsync.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{List{FileInfo}}"/>.</returns>
        public override Task<NCloudResult> GetFileInfosByPathAsync(string path)
        {
            return Task.Run(() =>
            {
                try
                {
                    var normalPath = helper.DenormalizePath(path);
                    path = Path.Combine(realPath, normalPath);
                    if (Directory.Exists(path))
                    {
                        return NCloudResult.OK(ResolveDir(path));
                    }
                    else if (File.Exists(path))
                    {
                        return NCloudResult.OK(ResolveFile(path));
                    }
                    else
                    {
                        return NCloudResult.Oops(ResultEnum.File_Opt_Failed);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    return NCloudResult.Oops(ResultEnum.Path_Unauthorized);
                }
            });
        }

        /// <summary>
        /// The ResolveFile.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="List{FileInfo}"/>.</returns>
        public FileInfo ResolveFile(string path)
        {
            var f = new System.IO.FileInfo(path);
            var fileInfo = new FileInfo
            {
                Name = f.Name,
                Size = f.Length,
                Type = "file",
            };

            return fileInfo;
        }

        /// <summary>
        /// The ResolveDir.
        /// </summary>
        /// <param name="path">The path<see cref="String"/>.</param>
        /// <returns>The <see cref="List{FileInfo}"/>.</returns>
        public List<FileInfo> ResolveDir(String path)
        {
            var di = new DirectoryInfo(path);
            var files = di.GetFileSystemInfos();
            var list = files.Select((f) =>
            {
                var fileInfo = new FileInfo
                {
                    Name = f.Name

                };
                if (f is DirectoryInfo)
                {
                    fileInfo.Type = "";
                    fileInfo.Size = 0;
                }
                else
                {
                    var fInfo = (System.IO.FileInfo)f;

                    fileInfo.Type = "file";
                    fileInfo.Size = fInfo.Length;
                }
                return fileInfo;
            }).ToList();
            return list;
        }
    }
}
