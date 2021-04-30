// -----------------------------------------------------------------------
// <copyright file="IContentDrive.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Core
{
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IContentDrive" />.
    /// </summary>
    public interface IContentDrive
    {
        /// <summary>
        /// The GetFileStream.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Task<Stream> GetFileStream(string path, bool cache = true);
    }
}
