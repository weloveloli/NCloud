// -----------------------------------------------------------------------
// <copyright file="PhysicalNCloudFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Physical
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.Support;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="PhysicalNCloudFileInfo" />.
    /// </summary>
    public class PhysicalNCloudFileInfo : FileInfoDecorator, IRandomAccessFileInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalNCloudFileInfo"/> class.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        public PhysicalNCloudFileInfo(IFileInfo fileInfo) : base(fileInfo)
        {
        }

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <param name="endPosition">The endPosition<see cref="long?"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream CreateReadStream(long startPosition, long? endPosition = null)
        {
            Check.CheckIndex(startPosition, endPosition, fileInfo.Length);
            var input = new FileStream(fileInfo.PhysicalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (startPosition != 0)
            {
                input.Seek(startPosition, SeekOrigin.Begin);
            }
            return input;
        }

        /// <summary>
        /// The CreateReadStreamAsync.
        /// </summary>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <param name="endPosition">The endPosition<see cref="long?"/>.</param>
        /// <param name="token">The token<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public Task<Stream> CreateReadStreamAsync(long startPosition, long? endPosition, CancellationToken token)
        {
            return Task.FromResult(this.CreateReadStream(startPosition, endPosition));
        }
    }
}
