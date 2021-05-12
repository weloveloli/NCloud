// -----------------------------------------------------------------------
// <copyright file="EmbeddableFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;

    /// <summary>
    /// Defines the <see cref="EmbeddedFileInfo" />.
    /// </summary>
    public class EmbeddedFileInfo : FileInfoDecorator
    {
        /// <summary>
        /// Defines the prefix.
        /// </summary>
        private readonly string prefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedFileInfo"/> class.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        public EmbeddedFileInfo(IFileInfo fileInfo, string prefix) : base(fileInfo)
        {
            this.prefix = prefix;
        }

        /// <summary>
        /// Gets the Length.
        /// </summary>
        public override long Length => 0;

        /// <summary>
        /// Gets a value indicating whether IsDirectory.
        /// </summary>
        public override bool IsDirectory => true;

        /// <summary>
        /// The GetSetting.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetProviderConfig()
        {
            return this.InnerIFileInfo.ReadAsString();
        }

        /// <summary>
        /// Gets the Prefix.
        /// </summary>
        public string Prefix => this.prefix;
    }
}
