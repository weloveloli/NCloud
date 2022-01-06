// -----------------------------------------------------------------------
// <copyright file="IExtendedFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    /// <summary>
    /// Defines the <see cref="IExtendedFileInfo" />.
    /// </summary>
    public interface IExtendedFileInfo : IAsyncReadFileInfo, IRandomAccessFileInfo
    {
        /// <summary>
        /// Gets the ETag.
        /// </summary>
        public string ETag { get; }
    }
}
