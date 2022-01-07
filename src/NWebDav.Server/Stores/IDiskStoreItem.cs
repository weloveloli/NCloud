// -----------------------------------------------------------------------
// <copyright file="IDiskStoreItem.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Stores
{
    /// <summary>
    /// Defines the <see cref="IDiskStoreItem" />.
    /// </summary>
    public interface IDiskStoreItem : IStoreItem
    {
        /// <summary>
        /// Gets a value indicating whether IsWritable.
        /// </summary>
        bool IsWritable { get; }

        /// <summary>
        /// Gets the FullPath.
        /// </summary>
        string FullPath { get; }
    }
}
