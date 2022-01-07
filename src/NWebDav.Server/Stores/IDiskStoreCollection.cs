// -----------------------------------------------------------------------
// <copyright file="IDiskStoreCollection.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Stores
{
    public interface IDiskStoreCollection : IStoreCollection
    {
        bool IsWritable { get; }
        string FullPath { get; }
    }
}
