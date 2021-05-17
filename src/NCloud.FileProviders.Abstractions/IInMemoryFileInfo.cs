// -----------------------------------------------------------------------
// <copyright file="IInMemoryFIleInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    /// <summary>
    /// Defines the <see cref="IInMemoryFileInfo" />.
    /// </summary>
    public interface IInMemoryFileInfo
    {
        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public byte[] GetBytes();
    }
}
