// -----------------------------------------------------------------------
// <copyright file="FileInfoExtensions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="FileInfoExtensions" />.
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Reads file content as string using <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ReadAsString(this IFileInfo fileInfo)
        {
            return fileInfo.ReadAsString(Encoding.UTF8);
        }

        /// <summary>
        /// Reads file content as string using <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public static Task<string> ReadAsStringAsync(this IFileInfo fileInfo)
        {
            return fileInfo.ReadAsStringAsync(Encoding.UTF8);
        }

        /// <summary>
        /// Reads file content as string using the given <paramref name="encoding"/>.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <param name="encoding">The encoding<see cref="Encoding"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ReadAsString(this IFileInfo fileInfo, Encoding encoding)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            if( fileInfo is InMemoryFileInfo memoryFileInfo)
            {
                return memoryFileInfo.ReadAsString(encoding);
            }
            using var stream = fileInfo.CreateReadStream();
            using var streamReader = new StreamReader(stream, encoding, true);
            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// Reads file content as string using the given <paramref name="encoding"/>.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <param name="encoding">The encoding<see cref="Encoding"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public static async Task<string> ReadAsStringAsync(this IFileInfo fileInfo, Encoding encoding)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            if (fileInfo is InMemoryFileInfo memoryFileInfo)
            {
                return memoryFileInfo.ReadAsString(encoding);
            }
            await using var stream = fileInfo.CreateReadStream();
            using var streamReader = new StreamReader(stream, encoding, true);
            return await streamReader.ReadToEndAsync();
        }

        /// <summary>
        /// Reads file content as byte[].
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] ReadBytes(this IFileInfo fileInfo)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            using var stream = fileInfo.CreateReadStream();
            return stream.GetAllBytes();
        }

        /// <summary>
        /// Reads file content as byte[].
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <returns>The <see cref="Task{byte[]}"/>.</returns>
        public static async Task<byte[]> ReadBytesAsync(this IFileInfo fileInfo)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            await using var stream = fileInfo.CreateReadStream();
            return await stream.GetAllBytesAsync();
        }

        /// <summary>
        /// The GetVirtualOrPhysicalPath.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <returns>The <see cref="string?"/>.</returns>
        public static string GetVirtualOrPhysicalPath(this IFileInfo fileInfo)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            if (fileInfo is IVirtualPathFileInfo virtualPathFileInfo)
            {
                return virtualPathFileInfo.GetVirtualPath();
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return GetVirtualOrPhysicalPath(decorator.InnerIFileInfo);
            }
            return fileInfo.PhysicalPath;
        }

        /// <summary>
        /// The GetVirtualOrPhysicalPath.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <returns>The <see cref="string?"/>.</returns>
        public static string GetRemoteUrl(this IFileInfo fileInfo)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            if (fileInfo is RemoteFileInfo remoteFileInfo)
            {
                return remoteFileInfo.GetRemoteUrl();
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return GetRemoteUrl(decorator.InnerIFileInfo);
            }
            return null;
        }
    }
}
