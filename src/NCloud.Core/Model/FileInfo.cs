// -----------------------------------------------------------------------
// <copyright file="FileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Core.Model
{
    using System;

    /// <summary>
    /// Defines the <see cref="FileInfo" />.
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Size.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the RemoteUrl.
        /// </summary>
        public string RemoteUrl { get; set; }
    }
}
