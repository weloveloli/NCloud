// -----------------------------------------------------------------------
// <copyright file="IStore.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Stores
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using NWebDav.Server.Http;
    using NWebDav.Server.Locking;
    using NWebDav.Server.Props;

    /// <summary>
    /// Defines the <see cref="StoreItemResult" />.
    /// </summary>
    public struct StoreItemResult
    {
        /// <summary>
        /// Gets the Result.
        /// </summary>
        public DavStatusCode Result { get; }

        /// <summary>
        /// Gets the Item.
        /// </summary>
        public IStoreItem Item { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref=""/> class.
        /// </summary>
        /// <param name="result">The result<see cref="DavStatusCode"/>.</param>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        public StoreItemResult(DavStatusCode result, IStoreItem item = null)
        {
            Result = result;
            Item = item;
        }


        public static bool operator !=(StoreItemResult left, StoreItemResult right)
        {
            return !(left == right);
        }

        public static bool operator ==(StoreItemResult left, StoreItemResult right)
        {
            return left.Result == right.Result && (left.Item == null && right.Item == null || left.Item != null && left.Item.Equals(right.Item));
        }
        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            return !(obj is StoreItemResult) ? false : this == (StoreItemResult)obj;
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode() => Result.GetHashCode() ^ (Item?.GetHashCode() ?? 0);
    }

    /// <summary>
    /// Defines the <see cref="StoreCollectionResult" />.
    /// </summary>
    public struct StoreCollectionResult
    {
        /// <summary>
        /// Gets the Result.
        /// </summary>
        public DavStatusCode Result { get; }

        /// <summary>
        /// Gets the Collection.
        /// </summary>
        public IStoreCollection Collection { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref=""/> class.
        /// </summary>
        /// <param name="result">The result<see cref="DavStatusCode"/>.</param>
        /// <param name="collection">The collection<see cref="IStoreCollection"/>.</param>
        public StoreCollectionResult(DavStatusCode result, IStoreCollection collection = null)
        {
            Result = result;
            Collection = collection;
        }


        public static bool operator !=(StoreCollectionResult left, StoreCollectionResult right)
        {
            return !(left == right);
        }

        public static bool operator ==(StoreCollectionResult left, StoreCollectionResult right)
        {
            return left.Result == right.Result && (left.Collection == null && right.Collection == null || left.Collection != null && left.Collection.Equals(right.Collection));
        }
        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            return !(obj is StoreCollectionResult) ? false : this == (StoreCollectionResult)obj;
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode() => Result.GetHashCode() ^ (Collection?.GetHashCode() ?? 0);
    }

    /// <summary>
    /// Defines the <see cref="IStore" />.
    /// </summary>
    public interface IStore
    {
        /// <summary>
        /// The GetItemAsync.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IStoreItem}"/>.</returns>
        Task<IStoreItem> GetItemAsync(Uri uri, IHttpContext httpContext);

        /// <summary>
        /// The GetCollectionAsync.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IStoreCollection}"/>.</returns>
        Task<IStoreCollection> GetCollectionAsync(Uri uri, IHttpContext httpContext);
    }

    /// <summary>
    /// Defines the <see cref="IStoreItem" />.
    /// </summary>
    public interface IStoreItem
    {
        // Item properties
        /// <summary>
        /// Gets the Name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the UniqueKey.
        /// </summary>
        string UniqueKey { get; }

        // Read/Write access to the data
        /// <summary>
        /// The GetReadableStreamAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        Task<Stream> GetReadableStreamAsync(IHttpContext httpContext);

        /// <summary>
        /// The UploadFromStreamAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="source">The source<see cref="Stream"/>.</param>
        /// <returns>The <see cref="Task{DavStatusCode}"/>.</returns>
        Task<DavStatusCode> UploadFromStreamAsync(IHttpContext httpContext, Stream source);

        // Copy support
        /// <summary>
        /// The CopyAsync.
        /// </summary>
        /// <param name="destination">The destination<see cref="IStoreCollection"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{StoreItemResult}"/>.</returns>
        Task<StoreItemResult> CopyAsync(IStoreCollection destination, string name, bool overwrite, IHttpContext httpContext);

        // Property support
        /// <summary>
        /// Gets the PropertyManager.
        /// </summary>
        IPropertyManager PropertyManager { get; }

        // Locking support
        /// <summary>
        /// Gets the LockingManager.
        /// </summary>
        ILockingManager LockingManager { get; }
    }

    /// <summary>
    /// Defines the <see cref="IStoreCollection" />.
    /// </summary>
    public interface IStoreCollection : IStoreItem
    {
        // Get specific item (or all items)
        /// <summary>
        /// The GetItemAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IStoreItem}"/>.</returns>
        Task<IStoreItem> GetItemAsync(string name, IHttpContext httpContext);

        /// <summary>
        /// The GetItemsAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{IStoreItem}}"/>.</returns>
        Task<IEnumerable<IStoreItem>> GetItemsAsync(IHttpContext httpContext);

        // Create items and collections and add to the collection
        /// <summary>
        /// The CreateItemAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{StoreItemResult}"/>.</returns>
        Task<StoreItemResult> CreateItemAsync(string name, bool overwrite, IHttpContext httpContext);

        /// <summary>
        /// The CreateCollectionAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{StoreCollectionResult}"/>.</returns>
        Task<StoreCollectionResult> CreateCollectionAsync(string name, bool overwrite, IHttpContext httpContext);

        // Checks if the collection can be moved directly to the destination
        /// <summary>
        /// The SupportsFastMove.
        /// </summary>
        /// <param name="destination">The destination<see cref="IStoreCollection"/>.</param>
        /// <param name="destinationName">The destinationName<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool SupportsFastMove(IStoreCollection destination, string destinationName, bool overwrite, IHttpContext httpContext);

        // Move items between collections
        /// <summary>
        /// The MoveItemAsync.
        /// </summary>
        /// <param name="sourceName">The sourceName<see cref="string"/>.</param>
        /// <param name="destination">The destination<see cref="IStoreCollection"/>.</param>
        /// <param name="destinationName">The destinationName<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{StoreItemResult}"/>.</returns>
        Task<StoreItemResult> MoveItemAsync(string sourceName, IStoreCollection destination, string destinationName, bool overwrite, IHttpContext httpContext);

        // Delete items from collection
        /// <summary>
        /// The DeleteItemAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{DavStatusCode}"/>.</returns>
        Task<DavStatusCode> DeleteItemAsync(string name, IHttpContext httpContext);

        /// <summary>
        /// Gets the InfiniteDepthMode.
        /// </summary>
        InfiniteDepthMode InfiniteDepthMode { get; }
    }

    /// <summary>
    /// When the Depth is set to infinite, then this enumeration specifies
    /// how to deal with this.
    /// </summary>
    public enum InfiniteDepthMode
    {
        /// <summary>
        /// Infinite depth is allowed (this is according spec).
        /// </summary>
        Allowed,
        /// <summary>
        /// Infinite depth is not allowed (this results in HTTP 403 Forbidden).
        /// </summary>
        Rejected,
        /// <summary>
        /// Infinite depth is handled as Depth 0.
        /// </summary>
        Assume0,
        /// <summary>
        /// Infinite depth is handled as Depth 1.
        /// </summary>
        Assume1
    }
}
