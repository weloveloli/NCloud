// -----------------------------------------------------------------------
// <copyright file="NCloudUnixFileSystemEntry.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP
{
    using System;
    using FubarDev.FtpServer.FileSystem;
    using FubarDev.FtpServer.FileSystem.Generic;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Support;

    /// <summary>
    /// Defines the <see cref="NCloudUnixFileSystemEntry" />.
    /// </summary>
    public abstract class NCloudUnixFileSystemEntry : FileInfoDecorator, IUnixFileSystemEntry
    {
        /// <summary>
        /// Defines the _defaultPermissions.
        /// </summary>
        private static readonly IUnixPermissions _defaultPermissions = new GenericUnixPermissions(new GenericAccessMode(true, false, false), new GenericAccessMode(true, false, false), new GenericAccessMode(true, false, false));

        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudUnixFileSystemEntry"/> class.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        protected NCloudUnixFileSystemEntry(IFileInfo fileInfo) : base(fileInfo)
        {
        }

        /// <summary>
        /// Gets the Permissions.
        /// </summary>
        public IUnixPermissions Permissions => _defaultPermissions;

        /// <summary>
        /// Gets the LastWriteTime.
        /// </summary>
        public DateTimeOffset? LastWriteTime => this.InnerIFileInfo.LastModified;

        /// <summary>
        /// Gets the CreatedTime.
        /// </summary>
        public DateTimeOffset? CreatedTime => this.InnerIFileInfo.LastModified;

        /// <summary>
        /// Gets the NumberOfLinks.
        /// </summary>
        public long NumberOfLinks => 1;

        /// <summary>
        /// Gets the Owner.
        /// </summary>
        public string Owner => "none";

        /// <summary>
        /// Gets the Group.
        /// </summary>
        public string Group => "none";
    }
}
