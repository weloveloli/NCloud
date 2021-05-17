// -----------------------------------------------------------------------
// <copyright file="NCloudFtpServerBuilderExtensions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP
{
    using FubarDev.FtpServer;
    using FubarDev.FtpServer.FileSystem;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Extension methods for <see cref="IFtpServerBuilder"/>.
    /// </summary>
    public static class NCloudFtpServerBuilderExtensions
    {
        /// <summary>
        /// Uses the .NET file system API..
        /// </summary>
        public static IFtpServerBuilder UseNCloudFileSystem(this IFtpServerBuilder builder)
        {
            return UseNCloudFileSystem<IFileProvider>(builder);
        }

        /// <summary>
        /// Uses the .NET file system API..
        /// </summary>
        public static IFtpServerBuilder UseNCloudFileSystem<T>(this IFtpServerBuilder builder) where T:IFileProvider
        {
            builder.Services.AddSingleton<IFileSystemClassFactory, NCloudFileSystemClassFactory>((sp) => { return new NCloudFileSystemClassFactory(sp.GetService<T>());});
            return builder;
        }
    }
}
