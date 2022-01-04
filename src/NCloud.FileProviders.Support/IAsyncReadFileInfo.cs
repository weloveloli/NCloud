// -----------------------------------------------------------------------
// <copyright file="IAsyncReadFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="IAsyncReadFileInfo" />.
    /// </summary>
    public interface IAsyncReadFileInfo : IFileInfo
    {
        /// <summary>
        /// The CreateReadStreamAsync.
        /// </summary>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public Task<Stream> CreateReadStreamAsync(CancellationToken cancellationToken = default);
    }
}
