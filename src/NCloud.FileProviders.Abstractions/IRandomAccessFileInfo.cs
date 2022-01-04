// -----------------------------------------------------------------------
// <copyright file="IRandomAccessFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="IRandomAccessFileInfo" />.
    /// </summary>
    public interface IRandomAccessFileInfo : IFileInfo
    {
        /// <summary>
        /// The OpenRead.
        /// </summary>
        /// <param name="startPosition">The positionStart<see cref="long"/>.</param>
        /// <param name="endPosition">The endPosition<see cref="long"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream CreateReadStream(long startPosition, long? endPosition = null);

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <param name="endPosition">The endPosition<see cref="long?"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Task<Stream> CreateReadStreamAsync(long startPosition, long? endPosition = null , CancellationToken token = default);
    }
}
