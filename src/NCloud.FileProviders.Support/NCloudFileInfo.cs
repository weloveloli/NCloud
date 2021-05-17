// -----------------------------------------------------------------------
// <copyright file="NCloudFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;

    /// <summary>
    /// Defines the <see cref="NCloudFileInfo" />.
    /// </summary>
    public class NCloudFileInfo : FileInfoDecorator, IVirtualPathFileInfo
    {
        /// <summary>
        /// Defines the path.
        /// </summary>
        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudFileInfo"/> class.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        public NCloudFileInfo(IFileInfo fileInfo, string path) : base(fileInfo)
        {
            this.path = path;
        }

        /// <summary>
        /// Gets the Path.
        /// </summary>
        public string Path => path;

        /// <summary>
        /// The GetVirtualPath.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetVirtualPath()
        {
            return Path;
        }
    }
}
