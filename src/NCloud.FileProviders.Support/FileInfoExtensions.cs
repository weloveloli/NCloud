// -----------------------------------------------------------------------
// <copyright file="FileInfoExtensions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System;
    using System.IO;
    using System.Net.Http.Headers;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
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

            if (fileInfo is IInMemoryFileInfo memoryFileInfo)
            {
                return encoding.GetString(memoryFileInfo.GetBytes());
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return ReadAsString(decorator.InnerIFileInfo);
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

            if (fileInfo is IInMemoryFileInfo memoryFileInfo)
            {
                return encoding.GetString(memoryFileInfo.GetBytes());
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return ReadAsString(decorator.InnerIFileInfo);
            }
            using var stream = await fileInfo.CreateReadStreamAsync();
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
            if (fileInfo is IInMemoryFileInfo memoryFileInfo)
            {
                return memoryFileInfo.GetBytes();
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return ReadBytes(decorator.InnerIFileInfo);
            }
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
            if (fileInfo is IInMemoryFileInfo memoryFileInfo)
            {
                return memoryFileInfo.GetBytes();
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return ReadBytes(decorator.InnerIFileInfo);
            }
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

            if (fileInfo is IRemoteFileInfo remoteFileInfo)
            {
                return remoteFileInfo.RemoteUrl;
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return GetRemoteUrl(decorator.InnerIFileInfo);
            }
            return null;
        }

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <param name="endPosition">The endPosition<see cref="long?"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public static Stream CreateReadStream(this IFileInfo fileInfo, long startPosition, long? endPosition)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            if (fileInfo is IExtendedFileInfo randomAccessFileInfo)
            {
                return randomAccessFileInfo.CreateReadStream(startPosition, endPosition);
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return CreateReadStream(decorator.InnerIFileInfo, startPosition, endPosition);
            }
            return null;
        }

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public static Task<Stream> CreateReadStreamAsync(this IFileInfo fileInfo)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            if (fileInfo is IAsyncReadFileInfo asyncReadFileInfo)
            {
                return asyncReadFileInfo.CreateReadStreamAsync();
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return CreateReadStreamAsync(decorator.InnerIFileInfo);
            }
            return Task.FromResult(fileInfo.CreateReadStream());
        }

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public static Task<Stream> CreateReadStreamAsync(this IFileInfo fileInfo, long startPosition)
        {
            if (startPosition == 0)
            {
                return fileInfo.CreateReadStreamAsync();
            }
            return CreateReadStreamAsync(fileInfo, startPosition, null, CancellationToken.None);
        }

        /// <summary>
        /// The IsRandomAccess.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsRandomAccess(this IFileInfo fileInfo)
        {
            if (fileInfo is IRandomAccessFileInfo)
            {
                return true;
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return IsRandomAccess(decorator.InnerIFileInfo);
            }
            return false;
        }

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <param name="endPosition">The endPosition<see cref="long?"/>.</param>
        /// <param name="token">The token<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public static Task<Stream> CreateReadStreamAsync(this IFileInfo fileInfo, long startPosition, long? endPosition, CancellationToken token = default)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            if (fileInfo is IRandomAccessFileInfo randomAccessFileInfo)
            {
                return randomAccessFileInfo.CreateReadStreamAsync(startPosition, endPosition, token);
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return CreateReadStreamAsync(decorator.InnerIFileInfo, startPosition, endPosition, token);
            }
            return null;
        }

        /// <summary>
        /// The DetermineContentType.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string DetermineContentType(this IFileInfo fileInfo)
        {
            return MimeTypeHelper.GetMimeType(fileInfo.Name);
        }

        /// <summary>
        /// The CalculateEtag.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string CalculateEtag(this IFileInfo fileInfo)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            if (fileInfo is IExtendedFileInfo extendedFileInfo)
            {
                return extendedFileInfo.ETag;
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return CalculateEtag(decorator.InnerIFileInfo);
            }
            long etagHash = fileInfo.LastModified.ToFileTime() ^ fileInfo.Length;
            return new EntityTagHeaderValue('\"' + Convert.ToString(etagHash, 16) + '\"').ToString();
        }

        public static bool SupportCustomHandleringWebDavGetRequest(this IFileInfo fileInfo)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            if (fileInfo is IWebDAVGetHandler extendedFileInfo)
            {
                return true;
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return SupportCustomHandleringWebDavGetRequest(decorator.InnerIFileInfo);
            }
            return false;
        }


        public static Task<bool> CustomHandleringWebDavGetRequest(this IFileInfo fileInfo, Microsoft.AspNetCore.Http.HttpContext ctx)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            if (fileInfo is IWebDAVGetHandler webDAVGetHandler)
            {
                return webDAVGetHandler.HandleWebDAVGetRequest(ctx);
            }
            if (fileInfo is FileInfoDecorator decorator)
            {
                return CustomHandleringWebDavGetRequest(decorator.InnerIFileInfo, ctx);
            }
            return Task.FromResult(true);
        }
    }
}
