// -----------------------------------------------------------------------
// <copyright file="IVirtualPathFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="IVirtualPathFileInfo" />.
    /// </summary>
    public interface IVirtualPathFileInfo : IFileInfo
    {
        /// <summary>
        /// The GetVirtualPath.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetVirtualPath();
    }
}
