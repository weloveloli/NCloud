// -----------------------------------------------------------------------
// <copyright file="DiskStoreCollection.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Stores
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using NWebDav.Server.Http;
    using NWebDav.Server.Locking;
    using NWebDav.Server.Logging;
    using NWebDav.Server.Props;

    /// <summary>
    /// Defines the <see cref="DiskStoreCollection" />.
    /// </summary>
    [DebuggerDisplay("{_directoryInfo.FullPath}\\")]
    public sealed class DiskStoreCollection : IDiskStoreCollection
    {
        /// <summary>
        /// Defines the s_log.
        /// </summary>
        private static readonly ILogger s_log = LoggerFactory.CreateLogger(typeof(DiskStoreCollection));

        /// <summary>
        /// Defines the s_xDavCollection.
        /// </summary>
        private static readonly XElement s_xDavCollection = new XElement(WebDavNamespaces.DavNs + "collection");

        /// <summary>
        /// Defines the _directoryInfo.
        /// </summary>
        private readonly DirectoryInfo _directoryInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskStoreCollection"/> class.
        /// </summary>
        /// <param name="lockingManager">The lockingManager<see cref="ILockingManager"/>.</param>
        /// <param name="directoryInfo">The directoryInfo<see cref="DirectoryInfo"/>.</param>
        /// <param name="isWritable">The isWritable<see cref="bool"/>.</param>
        public DiskStoreCollection(ILockingManager lockingManager, DirectoryInfo directoryInfo, bool isWritable)
        {
            LockingManager = lockingManager;
            _directoryInfo = directoryInfo;
            IsWritable = isWritable;
        }

        /// <summary>
        /// Gets the DefaultPropertyManager.
        /// </summary>
        public static PropertyManager<DiskStoreCollection> DefaultPropertyManager { get; } = new PropertyManager<DiskStoreCollection>(new DavProperty<DiskStoreCollection>[]
        {
            // RFC-2518 properties
            new DavCreationDate<DiskStoreCollection>
            {
                Getter = (context, collection) => collection._directoryInfo.CreationTimeUtc,
                Setter = (context, collection, value) =>
                {
                    collection._directoryInfo.CreationTimeUtc = value;
                    return DavStatusCode.Ok;
                }
            },
            new DavDisplayName<DiskStoreCollection>
            {
                Getter = (context, collection) => collection._directoryInfo.Name
            },
            new DavGetLastModified<DiskStoreCollection>
            {
                Getter = (context, collection) => collection._directoryInfo.LastWriteTimeUtc,
                Setter = (context, collection, value) =>
                {
                    collection._directoryInfo.LastWriteTimeUtc = value;
                    return DavStatusCode.Ok;
                }
            },
            new DavGetResourceType<DiskStoreCollection>
            {
                Getter = (context, collection) => new []{s_xDavCollection}
            },

            // Default locking property handling via the LockingManager
            new DavLockDiscoveryDefault<DiskStoreCollection>(),
            new DavSupportedLockDefault<DiskStoreCollection>(),

            // Hopmann/Lippert collection properties
            new DavExtCollectionChildCount<DiskStoreCollection>
            {
                Getter = (context, collection) => collection._directoryInfo.EnumerateFiles().Count() + collection._directoryInfo.EnumerateDirectories().Count()
            },
            new DavExtCollectionIsFolder<DiskStoreCollection>
            {
                Getter = (context, collection) => true
            },
            new DavExtCollectionIsHidden<DiskStoreCollection>
            {
                Getter = (context, collection) => (collection._directoryInfo.Attributes & FileAttributes.Hidden) != 0
            },
            new DavExtCollectionIsStructuredDocument<DiskStoreCollection>
            {
                Getter = (context, collection) => false
            },
            new DavExtCollectionHasSubs<DiskStoreCollection>
            {
                Getter = (context, collection) => collection._directoryInfo.EnumerateDirectories().Any()
            },
            new DavExtCollectionNoSubs<DiskStoreCollection>
            {
                Getter = (context, collection) => false
            },
            new DavExtCollectionObjectCount<DiskStoreCollection>
            {
                Getter = (context, collection) => collection._directoryInfo.EnumerateFiles().Count()
            },
            new DavExtCollectionReserved<DiskStoreCollection>
            {
                Getter = (context, collection) => !collection.IsWritable
            },
            new DavExtCollectionVisibleCount<DiskStoreCollection>
            {
                Getter = (context, collection) =>
                    collection._directoryInfo.EnumerateDirectories().Count(di => (di.Attributes & FileAttributes.Hidden) == 0) +
                    collection._directoryInfo.EnumerateFiles().Count(fi => (fi.Attributes & FileAttributes.Hidden) == 0)
            },

            // Win32 extensions
            new Win32CreationTime<DiskStoreCollection>
            {
                Getter = (context, collection) => collection._directoryInfo.CreationTimeUtc,
                Setter = (context, collection, value) =>
                {
                    collection._directoryInfo.CreationTimeUtc = value;
                    return DavStatusCode.Ok;
                }
            },
            new Win32LastAccessTime<DiskStoreCollection>
            {
                Getter = (context, collection) => collection._directoryInfo.LastAccessTimeUtc,
                Setter = (context, collection, value) =>
                {
                    collection._directoryInfo.LastAccessTimeUtc = value;
                    return DavStatusCode.Ok;
                }
            },
            new Win32LastModifiedTime<DiskStoreCollection>
            {
                Getter = (context, collection) => collection._directoryInfo.LastWriteTimeUtc,
                Setter = (context, collection, value) =>
                {
                    collection._directoryInfo.LastWriteTimeUtc = value;
                    return DavStatusCode.Ok;
                }
            },
            new Win32FileAttributes<DiskStoreCollection>
            {
                Getter = (context, collection) => collection._directoryInfo.Attributes,
                Setter = (context, collection, value) =>
                {
                    collection._directoryInfo.Attributes = value;
                    return DavStatusCode.Ok;
                }
            }
        });

        /// <summary>
        /// Gets a value indicating whether IsWritable.
        /// </summary>
        public bool IsWritable { get; }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name => _directoryInfo.Name;

        /// <summary>
        /// Gets the UniqueKey.
        /// </summary>
        public string UniqueKey => _directoryInfo.FullName;

        /// <summary>
        /// Gets the FullPath.
        /// </summary>
        public string FullPath => _directoryInfo.FullName;

        // Disk collections (a.k.a. directories don't have their own data)
        /// <summary>
        /// The GetReadableStreamAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public Task<Stream> GetReadableStreamAsync(IHttpContext httpContext) => Task.FromResult((Stream)null);

        /// <summary>
        /// The UploadFromStreamAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="inputStream">The inputStream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="Task{DavStatusCode}"/>.</returns>
        public Task<DavStatusCode> UploadFromStreamAsync(IHttpContext httpContext, Stream inputStream) => Task.FromResult(DavStatusCode.Conflict);

        /// <summary>
        /// Gets the PropertyManager.
        /// </summary>
        public IPropertyManager PropertyManager => DefaultPropertyManager;

        /// <summary>
        /// Gets the LockingManager.
        /// </summary>
        public ILockingManager LockingManager { get; }

        /// <summary>
        /// The GetItemAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IStoreItem}"/>.</returns>
        public Task<IStoreItem> GetItemAsync(string name, IHttpContext httpContext)
        {
            // Determine the full path
            var fullPath = Path.Combine(_directoryInfo.FullName, name);

            // Check if the item is a file
            if (File.Exists(fullPath))
                return Task.FromResult<IStoreItem>(new DiskStoreItem(LockingManager, new FileInfo(fullPath), IsWritable));

            // Check if the item is a directory
            if (Directory.Exists(fullPath))
                return Task.FromResult<IStoreItem>(new DiskStoreCollection(LockingManager, new DirectoryInfo(fullPath), IsWritable));

            // Item not found
            return Task.FromResult<IStoreItem>(null);
        }

        /// <summary>
        /// The GetItemsAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{IStoreItem}}"/>.</returns>
        public Task<IEnumerable<IStoreItem>> GetItemsAsync(IHttpContext httpContext)
        {
            IEnumerable<IStoreItem> GetItemsInternal()
            {
                // Add all directories
                foreach (var subDirectory in _directoryInfo.GetDirectories())
                    yield return new DiskStoreCollection(LockingManager, subDirectory, IsWritable);

                // Add all files
                foreach (var file in _directoryInfo.GetFiles())
                    yield return new DiskStoreItem(LockingManager, file, IsWritable);
            }

            return Task.FromResult(GetItemsInternal());
        }

        /// <summary>
        /// The CreateItemAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{StoreItemResult}"/>.</returns>
        public Task<StoreItemResult> CreateItemAsync(string name, bool overwrite, IHttpContext httpContext)
        {
            // Return error
            if (!IsWritable)
                return Task.FromResult(new StoreItemResult(DavStatusCode.PreconditionFailed));

            // Determine the destination path
            var destinationPath = Path.Combine(FullPath, name);

            // Determine result
            DavStatusCode result;

            // Check if the file can be overwritten
            if (File.Exists(name))
            {
                if (!overwrite)
                    return Task.FromResult(new StoreItemResult(DavStatusCode.PreconditionFailed));

                result = DavStatusCode.NoContent;
            }
            else
            {
                result = DavStatusCode.Created;
            }

            try
            {
                // Create a new file
                File.Create(destinationPath).Dispose();
            }
            catch (Exception exc)
            {
                // Log exception
                s_log.Log(LogLevel.Error, () => $"Unable to create '{destinationPath}' file.", exc);
                return Task.FromResult(new StoreItemResult(DavStatusCode.InternalServerError));
            }

            // Return result
            return Task.FromResult(new StoreItemResult(result, new DiskStoreItem(LockingManager, new FileInfo(destinationPath), IsWritable)));
        }

        /// <summary>
        /// The CreateCollectionAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{StoreCollectionResult}"/>.</returns>
        public Task<StoreCollectionResult> CreateCollectionAsync(string name, bool overwrite, IHttpContext httpContext)
        {
            // Return error
            if (!IsWritable)
                return Task.FromResult(new StoreCollectionResult(DavStatusCode.PreconditionFailed));

            // Determine the destination path
            var destinationPath = Path.Combine(FullPath, name);

            // Check if the directory can be overwritten
            DavStatusCode result;
            if (Directory.Exists(destinationPath))
            {
                // Check if overwrite is allowed
                if (!overwrite)
                    return Task.FromResult(new StoreCollectionResult(DavStatusCode.PreconditionFailed));

                // Overwrite existing
                result = DavStatusCode.NoContent;
            }
            else
            {
                // Created new directory
                result = DavStatusCode.Created;
            }

            try
            {
                // Attempt to create the directory
                Directory.CreateDirectory(destinationPath);
            }
            catch (Exception exc)
            {
                // Log exception
                s_log.Log(LogLevel.Error, () => $"Unable to create '{destinationPath}' directory.", exc);
                return null;
            }

            // Return the collection
            return Task.FromResult(new StoreCollectionResult(result, new DiskStoreCollection(LockingManager, new DirectoryInfo(destinationPath), IsWritable)));
        }

        /// <summary>
        /// The CopyAsync.
        /// </summary>
        /// <param name="destinationCollection">The destinationCollection<see cref="IStoreCollection"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{StoreItemResult}"/>.</returns>
        public async Task<StoreItemResult> CopyAsync(IStoreCollection destinationCollection, string name, bool overwrite, IHttpContext httpContext)
        {
            // Just create the folder itself
            var result = await destinationCollection.CreateCollectionAsync(name, overwrite, httpContext).ConfigureAwait(false);
            return new StoreItemResult(result.Result, result.Collection);
        }

        /// <summary>
        /// The SupportsFastMove.
        /// </summary>
        /// <param name="destination">The destination<see cref="IStoreCollection"/>.</param>
        /// <param name="destinationName">The destinationName<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool SupportsFastMove(IStoreCollection destination, string destinationName, bool overwrite, IHttpContext httpContext)
        {
            // We can only move disk-store collections
            return destination is DiskStoreCollection;
        }

        /// <summary>
        /// The MoveItemAsync.
        /// </summary>
        /// <param name="sourceName">The sourceName<see cref="string"/>.</param>
        /// <param name="destinationCollection">The destinationCollection<see cref="IStoreCollection"/>.</param>
        /// <param name="destinationName">The destinationName<see cref="string"/>.</param>
        /// <param name="overwrite">The overwrite<see cref="bool"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{StoreItemResult}"/>.</returns>
        public async Task<StoreItemResult> MoveItemAsync(string sourceName, IStoreCollection destinationCollection, string destinationName, bool overwrite, IHttpContext httpContext)
        {
            // Return error
            if (!IsWritable)
                return new StoreItemResult(DavStatusCode.PreconditionFailed);

            // Determine the object that is being moved
            var item = await GetItemAsync(sourceName, httpContext).ConfigureAwait(false);
            if (item == null)
                return new StoreItemResult(DavStatusCode.NotFound);

            try
            {
                // If the destination collection is a directory too, then we can simply move the file
                if (destinationCollection is DiskStoreCollection destinationDiskStoreCollection)
                {
                    // Return error
                    if (!destinationDiskStoreCollection.IsWritable)
                        return new StoreItemResult(DavStatusCode.PreconditionFailed);

                    // Determine source and destination paths
                    var sourcePath = Path.Combine(_directoryInfo.FullName, sourceName);
                    var destinationPath = Path.Combine(destinationDiskStoreCollection._directoryInfo.FullName, destinationName);

                    // Check if the file already exists
                    DavStatusCode result;
                    if (File.Exists(destinationPath))
                    {
                        // Remove the file if it already exists (if allowed)
                        if (!overwrite)
                            return new StoreItemResult(DavStatusCode.Forbidden);

                        // The file will be overwritten
                        File.Delete(destinationPath);
                        result = DavStatusCode.NoContent;
                    }
                    else if (Directory.Exists(destinationPath))
                    {
                        // Remove the directory if it already exists (if allowed)
                        if (!overwrite)
                            return new StoreItemResult(DavStatusCode.Forbidden);

                        // The file will be overwritten
                        Directory.Delete(destinationPath, true);
                        result = DavStatusCode.NoContent;
                    }
                    else
                    {
                        // The file will be "created"
                        result = DavStatusCode.Created;
                    }

                    switch (item)
                    {
                        case DiskStoreItem _:
                            // Move the file
                            File.Move(sourcePath, destinationPath);
                            return new StoreItemResult(result, new DiskStoreItem(LockingManager, new FileInfo(destinationPath), IsWritable));

                        case DiskStoreCollection _:
                            // Move the directory
                            Directory.Move(sourcePath, destinationPath);
                            return new StoreItemResult(result, new DiskStoreCollection(LockingManager, new DirectoryInfo(destinationPath), IsWritable));

                        default:
                            // Invalid item
                            Debug.Fail($"Invalid item {item.GetType()} inside the {nameof(DiskStoreCollection)}.");
                            return new StoreItemResult(DavStatusCode.InternalServerError);
                    }
                }
                else
                {
                    // Attempt to copy the item to the destination collection
                    var result = await item.CopyAsync(destinationCollection, destinationName, overwrite, httpContext).ConfigureAwait(false);
                    if (result.Result == DavStatusCode.Created || result.Result == DavStatusCode.NoContent)
                        await DeleteItemAsync(sourceName, httpContext).ConfigureAwait(false);

                    // Return the result
                    return result;
                }
            }
            catch (UnauthorizedAccessException)
            {
                return new StoreItemResult(DavStatusCode.Forbidden);
            }
        }

        /// <summary>
        /// The DeleteItemAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <returns>The <see cref="Task{DavStatusCode}"/>.</returns>
        public Task<DavStatusCode> DeleteItemAsync(string name, IHttpContext httpContext)
        {
            // Return error
            if (!IsWritable)
                return Task.FromResult(DavStatusCode.PreconditionFailed);

            // Determine the full path
            var fullPath = Path.Combine(_directoryInfo.FullName, name);
            try
            {
                // Check if the file exists
                if (File.Exists(fullPath))
                {
                    // Delete the file
                    File.Delete(fullPath);
                    return Task.FromResult(DavStatusCode.Ok);
                }

                // Check if the directory exists
                if (Directory.Exists(fullPath))
                {
                    // Delete the directory
                    Directory.Delete(fullPath);
                    return Task.FromResult(DavStatusCode.Ok);
                }

                // Item not found
                return Task.FromResult(DavStatusCode.NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                return Task.FromResult(DavStatusCode.Forbidden);
            }
            catch (Exception exc)
            {
                // Log exception
                s_log.Log(LogLevel.Error, () => $"Unable to delete '{fullPath}' directory.", exc);
                return Task.FromResult(DavStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets the InfiniteDepthMode.
        /// </summary>
        public InfiniteDepthMode InfiniteDepthMode => InfiniteDepthMode.Rejected;

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            return _directoryInfo.FullName.GetHashCode();
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            var storeCollection = obj as DiskStoreCollection;
            if (storeCollection == null)
                return false;
            return storeCollection._directoryInfo.FullName.Equals(_directoryInfo.FullName, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
