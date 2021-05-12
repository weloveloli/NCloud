// -----------------------------------------------------------------------
// <copyright file="NCloudFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="NCloudFileInfo" />.
    /// </summary>
    public class NCloudFileInfo : FileInfoDecorator
    {
        /// <summary>
        /// Defines the subPath.
        /// </summary>
        private readonly string subPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudFileInfo"/> class.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <param name="subPath">The subPath<see cref="string"/>.</param>
        public NCloudFileInfo(IFileInfo fileInfo, string subPath) : base(fileInfo)
        {
            this.subPath = subPath;
        }

        /// <summary>
        /// Gets the Path.
        /// </summary>
        public string Path => subPath;
    }
}
