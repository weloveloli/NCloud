// -----------------------------------------------------------------------
// <copyright file="DiskStore.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Stores
{
    using System;
    using System.IO;
    using System.Security;
    using System.Threading.Tasks;
    using NWebDav.Server.Helpers;
    using NWebDav.Server.Http;
    using NWebDav.Server.Locking;

    /// <summary>
    /// Defines the <see cref="DiskStore" />.
    /// </summary>
    public sealed class DiskStore : IStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiskStore"/> class.
        /// </summary>
        /// <param name="directory">The directory<see cref="string"/>.</param>
        /// <param name="isWritable">The isWritable<see cref="bool"/>.</param>
        /// <param name="lockingManager">The lockingManager<see cref="ILockingManager"/>.</param>
        public DiskStore(string directory, bool isWritable = true, ILockingManager lockingManager = null)
        {
            BaseDirectory = directory ?? throw new ArgumentNullException(nameof(directory));
            IsWritable = isWritable;
            LockingManager = lockingManager ?? new InMemoryLockingManager();
        }

        /// <summary>
        /// Gets the BaseDirectory.
        /// </summary>
        public string BaseDirectory { get; }

        /// <summary>
        /// Gets a value indicating whether IsWritable.
        /// </summary>
        public bool IsWritable { get; }

        /// <summary>
        /// Gets the LockingManager.
        /// </summary>
        public ILockingManager LockingManager { get; }

        /// <summary>
        /// The GetItemAsync.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IStoreItem}"/>.</returns>
        public Task<IStoreItem> GetItemAsync(Uri uri, IHttpContext httpContext)
        {
            // Determine the path from the uri
            var path = GetPathFromUri(uri);

            // Check if it's a directory
            if (Directory.Exists(path))
            {
                return Task.FromResult<IStoreItem>(new DiskStoreCollection(LockingManager, new DirectoryInfo(path), IsWritable));
            }

            // Check if it's a file
            if (File.Exists(path))
            {
                return Task.FromResult<IStoreItem>(new DiskStoreItem(LockingManager, new FileInfo(path), IsWritable));
            }

            // The item doesn't exist
            return Task.FromResult<IStoreItem>(null);
        }

        /// <summary>
        /// The GetCollectionAsync.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IStoreCollection}"/>.</returns>
        public Task<IStoreCollection> GetCollectionAsync(Uri uri, IHttpContext httpContext)
        {
            // Determine the path from the uri
            var path = GetPathFromUri(uri);
            if (!Directory.Exists(path))
            {
                return Task.FromResult<IStoreCollection>(null);
            }

            // Return the item
            return Task.FromResult<IStoreCollection>(new DiskStoreCollection(LockingManager, new DirectoryInfo(path), IsWritable));
        }

        /// <summary>
        /// The GetPathFromUri.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string GetPathFromUri(Uri uri)
        {
            // Determine the path
            var requestedPath = UriHelper.GetDecodedPath(uri).Substring(1).Replace('/', Path.DirectorySeparatorChar);

            // Determine the full path
            var fullPath = Path.GetFullPath(Path.Combine(BaseDirectory, requestedPath));

            // Make sure we're still inside the specified directory
            if (fullPath != BaseDirectory && !fullPath.StartsWith(BaseDirectory + Path.DirectorySeparatorChar))
            {
                throw new SecurityException($"Uri '{uri}' is outside the '{BaseDirectory}' directory.");
            }

            // Return the combined path
            return fullPath;
        }
    }
}
