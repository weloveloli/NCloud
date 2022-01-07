// -----------------------------------------------------------------------
// <copyright file="MultiStore.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Stores
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NWebDav.Server.Helpers;
    using NWebDav.Server.Http;

    /// <summary>
    /// Defines the <see cref="MultiStore" />.
    /// </summary>
    public class MultiStore : IStore
    {
        /// <summary>
        /// Defines the _storeResolvers.
        /// </summary>
        private readonly IDictionary<string, IStore> _storeResolvers = new Dictionary<string, IStore>();

        /// <summary>
        /// The AddStore.
        /// </summary>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        /// <param name="store">The store<see cref="IStore"/>.</param>
        public void AddStore(string prefix, IStore store)
        {
            // Convert the prefix to lower-case
            prefix = prefix.ToLowerInvariant();

            // Add the prefix to the store
            _storeResolvers.Add(prefix, store);
        }

        /// <summary>
        /// The RemoveStore.
        /// </summary>
        /// <param name="prefix">The prefix<see cref="string"/>.</param>
        public void RemoveStore(string prefix)
        {
            // Convert the prefix to lower-case
            prefix = prefix.ToLowerInvariant();

            // Add the prefix to the store
            _storeResolvers.Remove(prefix);
        }

        /// <summary>
        /// The GetItemAsync.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IStoreItem}"/>.</returns>
        public Task<IStoreItem> GetItemAsync(Uri uri, IHttpContext httpContext)
        {
            return Resolve(uri, (storeResolver, subUri) => storeResolver.GetItemAsync(subUri, httpContext));
        }

        /// <summary>
        /// The GetCollectionAsync.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IStoreCollection}"/>.</returns>
        public Task<IStoreCollection> GetCollectionAsync(Uri uri, IHttpContext httpContext)
        {
            return Resolve(uri, (storeResolver, subUri) => storeResolver.GetCollectionAsync(subUri, httpContext));
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="action">The action<see cref="Func{IStore, Uri, T}"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        private T Resolve<T>(Uri uri, Func<IStore, Uri, T> action)
        {
            // Determine the path
            var requestedPath = uri.LocalPath;
            var endOfPrefix = requestedPath.IndexOf('/');
            var prefix = (endOfPrefix >= 0 ? requestedPath.Substring(0, endOfPrefix) : requestedPath).ToLowerInvariant();
            var subUri = UriHelper.Combine(uri, endOfPrefix >= 0 ? requestedPath.Substring(endOfPrefix + 1) : string.Empty);

            // Try to find the store
            IStore store;
            if (!_storeResolvers.TryGetValue(prefix, out store))
                return default(T);

            // Resolve via the action
            return action(store, subUri);
        }
    }
}
