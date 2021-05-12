// -----------------------------------------------------------------------
// <copyright file="TestDirFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System;
    using System.IO;
    using NCloud.FileProviders.Abstractions;

    /// <summary>
    /// Defines the <see cref="VirtualFileInfo" />.
    /// </summary>
    public class VirtualFileInfo : IVirtualPathFileInfo
    {
        /// <summary>
        /// Defines the name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Defines the path.
        /// </summary>
        private readonly string path;

        /// <summary>
        /// Defines the isDirectory.
        /// </summary>
        private readonly bool isDirectory;

        /// <summary>
        /// Defines the length.
        /// </summary>
        private readonly int length;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualFileInfo"/> class.
        /// </summary>
        /// <param name="relpath">The path<see cref="string"/>.</param>
        /// <param name="isDirectory">The isDirectory<see cref="bool"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        public VirtualFileInfo(string relpath, bool isDirectory = true, int length = 0)
        {
            this.path = relpath;
            this.isDirectory = isDirectory;
            this.length = length;
            this.name = Path.GetFileName(path);
            if (isDirectory)
            {
                //this.name = this.name.EnsureEndsWith("/");
            }
        }

        /// <summary>
        /// Gets a value indicating whether Exists.
        /// </summary>
        public bool Exists => true;

        /// <summary>
        /// Gets the Length.
        /// </summary>
        public long Length => length;

        /// <summary>
        /// Gets the PhysicalPath.
        /// </summary>
        public string PhysicalPath => null;

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name => this.name;

        /// <summary>
        /// Gets the LastModified.
        /// </summary>
        public DateTimeOffset LastModified => DateTimeOffset.Now;

        /// <summary>
        /// Gets a value indicating whether IsDirectory.
        /// </summary>
        public bool IsDirectory => isDirectory;

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream CreateReadStream()
        {
            return null;
        }

        /// <summary>
        /// The GetVirtualPath.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetVirtualPath()
        {
            return this.path;
        }
    }
}
