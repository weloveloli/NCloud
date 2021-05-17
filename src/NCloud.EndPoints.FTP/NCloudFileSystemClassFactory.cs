// -----------------------------------------------------------------------
// <copyright file="NCloudFileSystemClassFactory.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP
{
    using System;
    using System.Threading.Tasks;
    using FubarDev.FtpServer;
    using FubarDev.FtpServer.FileSystem;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="NCloudFileSystemClassFactory" />.
    /// </summary>
    public class NCloudFileSystemClassFactory : IFileSystemClassFactory
    {
        /// <summary>
        /// Defines the fileProvider.
        /// </summary>
        private readonly IFileProvider fileProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudFileSystemClassFactory"/> class.
        /// </summary>
        /// <param name="fileProvider">The fileProvider<see cref="IFileProvider"/>.</param>
        public NCloudFileSystemClassFactory(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }

        /// <summary>
        /// The Create.
        /// </summary>
        /// <param name="accountInformation">The accountInformation<see cref="IAccountInformation"/>.</param>
        /// <returns>The <see cref="Task{IUnixFileSystem}"/>.</returns>
        public Task<IUnixFileSystem> Create(IAccountInformation accountInformation)
        {
            return Task.FromResult((IUnixFileSystem)new NCloudFileSystem(this.fileProvider));
        }
    }
}
