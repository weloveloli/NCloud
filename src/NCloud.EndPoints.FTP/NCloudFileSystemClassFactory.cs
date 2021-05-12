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

    /// <summary>
    /// Defines the <see cref="NCloudFileSystemClassFactory" />.
    /// </summary>
    public class NCloudFileSystemClassFactory : IFileSystemClassFactory
    {
        /// <summary>
        /// The Create.
        /// </summary>
        /// <param name="accountInformation">The accountInformation<see cref="IAccountInformation"/>.</param>
        /// <returns>The <see cref="Task{IUnixFileSystem}"/>.</returns>
        public Task<IUnixFileSystem> Create(IAccountInformation accountInformation)
        {
            throw new NotImplementedException();
        }
    }
}
