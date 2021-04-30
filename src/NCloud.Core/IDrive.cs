// -----------------------------------------------------------------------
// <copyright file="IDrive.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="IDrive" />.
    /// </summary>
    public interface IDrive
    {
        /// <summary>
        /// The GetFileInfosByPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="List{FileInfo}"/>.</returns>
        public Task<NCloudResult> GetFileInfosByPathAsync(string path);

        /// <summary>
        /// The GetConfig.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetConfig();

        /// <summary>
        /// The GetFileStreamByPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="cache">The cache<see cref="bool"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Task<Stream> GetFileStreamByPathAsync(string path, bool cache = true);
    }
}
