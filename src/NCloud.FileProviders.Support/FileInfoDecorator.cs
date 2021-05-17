// -----------------------------------------------------------------------
// <copyright file="FileInfoDecorator.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System;
    using System.IO;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="FileInfoDecorator" />.
    /// </summary>
    public abstract class FileInfoDecorator : IFileInfo
    {
        /// <summary>
        /// Defines the fileInfo.
        /// </summary>
        protected readonly IFileInfo fileInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfoDecorator"/> class.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        public FileInfoDecorator(IFileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        /// <summary>
        /// Gets the InnerIFileInfo.
        /// </summary>
        public IFileInfo InnerIFileInfo => fileInfo;

        /// <summary>
        /// Gets a value indicating whether Exists.
        /// </summary>
        public virtual bool Exists => fileInfo.Exists;

        /// <summary>
        /// Gets the Length.
        /// </summary>
        public virtual long Length => fileInfo.Length;

        /// <summary>
        /// Gets the PhysicalPath.
        /// </summary>
        public virtual string PhysicalPath => fileInfo.PhysicalPath;

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public virtual string Name => fileInfo.Name;

        /// <summary>
        /// Gets the LastModified.
        /// </summary>
        public virtual DateTimeOffset LastModified => fileInfo.LastModified;

        /// <summary>
        /// Gets a value indicating whether IsDirectory.
        /// </summary>
        public virtual bool IsDirectory => fileInfo.IsDirectory;

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <returns>The <see cref="Stream"/>.</returns>
        public virtual Stream CreateReadStream()
        {
            return fileInfo.CreateReadStream();
        }
    }
}
