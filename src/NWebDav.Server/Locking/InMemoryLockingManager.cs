// -----------------------------------------------------------------------
// <copyright file="InMemoryLockingManager.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Locking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using NWebDav.Server.Stores;

    // TODO: Remove auto-expired locks
    // TODO: Add support for recursive locks
    /// <summary>
    /// Defines the <see cref="InMemoryLockingManager" />.
    /// </summary>
    public class InMemoryLockingManager : ILockingManager
    {
        /// <summary>
        /// Defines the <see cref="ItemLockInfo" />.
        /// </summary>
        private class ItemLockInfo
        {
            /// <summary>
            /// Gets the Token.
            /// </summary>
            public Guid Token { get; }

            /// <summary>
            /// Gets the Item.
            /// </summary>
            public IStoreItem Item { get; }

            /// <summary>
            /// Gets the Type.
            /// </summary>
            public LockType Type { get; }

            /// <summary>
            /// Gets the Scope.
            /// </summary>
            public LockScope Scope { get; }

            /// <summary>
            /// Gets the LockRootUri.
            /// </summary>
            public Uri LockRootUri { get; }

            /// <summary>
            /// Gets a value indicating whether Recursive.
            /// </summary>
            public bool Recursive { get; }

            /// <summary>
            /// Gets the Owner.
            /// </summary>
            public XElement Owner { get; }

            /// <summary>
            /// Gets the Timeout.
            /// </summary>
            public int Timeout { get; }

            /// <summary>
            /// Gets the Expires.
            /// </summary>
            public DateTime? Expires { get; private set; }

            /// <summary>
            /// Gets a value indicating whether IsExpired.
            /// </summary>
            public bool IsExpired => !Expires.HasValue || Expires < DateTime.UtcNow;

            /// <summary>
            /// Initializes a new instance of the <see cref="ItemLockInfo"/> class.
            /// </summary>
            /// <param name="item">The item<see cref="IStoreItem"/>.</param>
            /// <param name="lockType">The lockType<see cref="LockType"/>.</param>
            /// <param name="lockScope">The lockScope<see cref="LockScope"/>.</param>
            /// <param name="lockRootUri">The lockRootUri<see cref="Uri"/>.</param>
            /// <param name="recursive">The recursive<see cref="bool"/>.</param>
            /// <param name="owner">The owner<see cref="XElement"/>.</param>
            /// <param name="timeout">The timeout<see cref="int"/>.</param>
            public ItemLockInfo(IStoreItem item, LockType lockType, LockScope lockScope, Uri lockRootUri, bool recursive, XElement owner, int timeout)
            {
                Token = Guid.NewGuid();
                Item = item;
                Type = lockType;
                Scope = lockScope;
                LockRootUri = lockRootUri;
                Recursive = recursive;
                Owner = owner;
                Timeout = timeout;

                RefreshExpiration(timeout);
            }

            /// <summary>
            /// The RefreshExpiration.
            /// </summary>
            /// <param name="timeout">The timeout<see cref="int"/>.</param>
            public void RefreshExpiration(int timeout)
            {
                Expires = timeout >= 0 ? (DateTime?)DateTime.UtcNow.AddSeconds(timeout) : null;
            }
        }

        /// <summary>
        /// Defines the <see cref="ItemLockList" />.
        /// </summary>
        private class ItemLockList : List<ItemLockInfo>
        {
        }

        /// <summary>
        /// Defines the <see cref="ItemLockTypeDictionary" />.
        /// </summary>
        private class ItemLockTypeDictionary : Dictionary<LockType, ItemLockList>
        {
        }

        /// <summary>
        /// Defines the TokenScheme.
        /// </summary>
        private const string TokenScheme = "opaquelocktoken";

        /// <summary>
        /// Defines the _itemLocks.
        /// </summary>
        private readonly IDictionary<string, ItemLockTypeDictionary> _itemLocks = new Dictionary<string, ItemLockTypeDictionary>();

        /// <summary>
        /// Defines the s_supportedLocks.
        /// </summary>
        private static readonly LockEntry[] s_supportedLocks =
        {
            new LockEntry(LockScope.Exclusive, LockType.Write),
            new LockEntry(LockScope.Shared, LockType.Write)
        };

        /// <summary>
        /// The Lock.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <param name="lockType">The lockType<see cref="LockType"/>.</param>
        /// <param name="lockScope">The lockScope<see cref="LockScope"/>.</param>
        /// <param name="owner">The owner<see cref="XElement"/>.</param>
        /// <param name="lockRootUri">The lockRootUri<see cref="Uri"/>.</param>
        /// <param name="recursive">The recursive<see cref="bool"/>.</param>
        /// <param name="timeouts">The timeouts<see cref="IEnumerable{int}"/>.</param>
        /// <returns>The <see cref="LockResult"/>.</returns>
        public LockResult Lock(IStoreItem item, LockType lockType, LockScope lockScope, XElement owner, Uri lockRootUri, bool recursive, IEnumerable<int> timeouts)
        {
            // Determine the expiration based on the first time-out
            var timeout = timeouts.Cast<int?>().FirstOrDefault();

            // Determine the item's key
            var key = item.UniqueKey;

            lock (_itemLocks)
            {
                // Make sure the item is in the dictionary
                if (!_itemLocks.TryGetValue(key, out var itemLockTypeDictionary))
                    _itemLocks.Add(key, itemLockTypeDictionary = new ItemLockTypeDictionary());

                // Make sure there is already a lock-list for this type
                if (!itemLockTypeDictionary.TryGetValue(lockType, out var itemLockList))
                {
                    // Create a new lock-list
                    itemLockTypeDictionary.Add(lockType, itemLockList = new ItemLockList());
                }
                else
                {
                    // Check if there is already an exclusive lock
                    if (itemLockList.Any(l => l.Scope == LockScope.Exclusive))
                        return new LockResult(DavStatusCode.Locked);
                }

                // Create the lock info object
                var itemLockInfo = new ItemLockInfo(item, lockType, lockScope, lockRootUri, recursive, owner, timeout ?? -1);

                // Add the lock
                itemLockList.Add(itemLockInfo);

                // Return the active lock
                return new LockResult(DavStatusCode.Ok, GetActiveLockInfo(itemLockInfo));
            }
        }

        /// <summary>
        /// The Unlock.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <param name="lockTokenUri">The lockTokenUri<see cref="Uri"/>.</param>
        /// <returns>The <see cref="DavStatusCode"/>.</returns>
        public DavStatusCode Unlock(IStoreItem item, Uri lockTokenUri)
        {
            // Determine the actual lock token
            var lockToken = GetTokenFromLockToken(lockTokenUri);
            if (lockToken == null)
                return DavStatusCode.PreconditionFailed;

            // Determine the item's key
            var key = item.UniqueKey;

            lock (_itemLocks)
            {
                // Make sure the item is in the dictionary
                if (!_itemLocks.TryGetValue(key, out var itemLockTypeDictionary))
                    return DavStatusCode.PreconditionFailed;

                // Scan both the dictionaries for the token
                foreach (var kv in itemLockTypeDictionary)
                {
                    var itemLockList = kv.Value;

                    // Remove this lock from the list
                    for (var i = 0; i < itemLockList.Count; ++i)
                    {
                        if (itemLockList[i].Token == lockToken.Value)
                        {
                            // Remove the item
                            itemLockList.RemoveAt(i);

                            // Check if there are any locks left for this type
                            if (!itemLockList.Any())
                            {
                                // Remove the type
                                itemLockTypeDictionary.Remove(kv.Key);

                                // Check if there are any types left
                                if (!itemLockTypeDictionary.Any())
                                    _itemLocks.Remove(key);
                            }

                            // Lock has been removed
                            return DavStatusCode.NoContent;
                        }
                    }
                }
            }

            // Item cannot be unlocked (token cannot be found)
            return DavStatusCode.PreconditionFailed;
        }

        /// <summary>
        /// The RefreshLock.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <param name="recursiveLock">The recursiveLock<see cref="bool"/>.</param>
        /// <param name="timeouts">The timeouts<see cref="IEnumerable{int}"/>.</param>
        /// <param name="lockTokenUri">The lockTokenUri<see cref="Uri"/>.</param>
        /// <returns>The <see cref="LockResult"/>.</returns>
        public LockResult RefreshLock(IStoreItem item, bool recursiveLock, IEnumerable<int> timeouts, Uri lockTokenUri)
        {
            // Determine the actual lock token
            var lockToken = GetTokenFromLockToken(lockTokenUri);
            if (lockToken == null)
                return new LockResult(DavStatusCode.PreconditionFailed);

            // Determine the item's key
            var key = item.UniqueKey;

            lock (_itemLocks)
            {
                // Make sure the item is in the dictionary
                if (!_itemLocks.TryGetValue(key, out var itemLockTypeDictionary))
                    return new LockResult(DavStatusCode.PreconditionFailed);

                // Scan both the dictionaries for the token
                foreach (var kv in itemLockTypeDictionary)
                {
                    // Refresh the lock
                    var itemLockInfo = kv.Value.FirstOrDefault(lt => lt.Token == lockToken.Value && !lt.IsExpired);
                    if (itemLockInfo != null)
                    {
                        // Determine the expiration based on the first time-out
                        var timeout = timeouts.Cast<int?>().FirstOrDefault() ?? itemLockInfo.Timeout;
                        itemLockInfo.RefreshExpiration(timeout);

                        // Return the active lock
                        return new LockResult(DavStatusCode.Ok, GetActiveLockInfo(itemLockInfo));
                    }
                }
            }

            // Item cannot be unlocked (token cannot be found)
            return new LockResult(DavStatusCode.PreconditionFailed);
        }

        /// <summary>
        /// The GetActiveLockInfo.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <returns>The <see cref="IEnumerable{ActiveLock}"/>.</returns>
        public IEnumerable<ActiveLock> GetActiveLockInfo(IStoreItem item)
        {
            // Determine the item's key
            var key = item.UniqueKey;

            lock (_itemLocks)
            {
                // Make sure the item is in the dictionary
                if (!_itemLocks.TryGetValue(key, out var itemLockTypeDictionary))
                    return new ActiveLock[0];

                // Return all non-expired locks
                return itemLockTypeDictionary.SelectMany(kv => kv.Value).Where(l => !l.IsExpired).Select(GetActiveLockInfo).ToList();
            }
        }

        /// <summary>
        /// The GetSupportedLocks.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <returns>The <see cref="IEnumerable{LockEntry}"/>.</returns>
        public IEnumerable<LockEntry> GetSupportedLocks(IStoreItem item)
        {
            // We support both shared and exclusive locks for items and collections
            return s_supportedLocks;
        }

        /// <summary>
        /// The IsLocked.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsLocked(IStoreItem item)
        {
            // Determine the item's key
            var key = item.UniqueKey;

            lock (_itemLocks)
            {
                // Make sure the item is in the dictionary
                if (_itemLocks.TryGetValue(key, out var itemLockTypeDictionary))
                {
                    foreach (var kv in itemLockTypeDictionary)
                    {
                        if (kv.Value.Any(li => !li.IsExpired))
                            return true;
                    }
                }
            }

            // No lock
            return false;
        }

        /// <summary>
        /// The HasLock.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <param name="lockTokenUri">The lockTokenUri<see cref="Uri"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool HasLock(IStoreItem item, Uri lockTokenUri)
        {
            // If no lock is specified, then we should abort
            if (lockTokenUri == null)
                return false;

            // Determine the item's key
            var key = item.UniqueKey;

            // Determine the actual lock token
            var lockToken = GetTokenFromLockToken(lockTokenUri);
            if (lockToken == null)
                return false;

            lock (_itemLocks)
            {
                // Make sure the item is in the dictionary
                if (!_itemLocks.TryGetValue(key, out var itemLockTypeDictionary))
                    return false;

                // Scan both the dictionaries for the token
                foreach (var kv in itemLockTypeDictionary)
                {
                    // Refresh the lock
                    var itemLockInfo = kv.Value.FirstOrDefault(lt => lt.Token == lockToken.Value && !lt.IsExpired);
                    if (itemLockInfo != null)
                        return true;
                }
            }

            // No lock
            return false;
        }

        /// <summary>
        /// The GetActiveLockInfo.
        /// </summary>
        /// <param name="itemLockInfo">The itemLockInfo<see cref="ItemLockInfo"/>.</param>
        /// <returns>The <see cref="ActiveLock"/>.</returns>
        private static ActiveLock GetActiveLockInfo(ItemLockInfo itemLockInfo)
        {
            return new ActiveLock(itemLockInfo.Type, itemLockInfo.Scope, itemLockInfo.Recursive ? int.MaxValue : 0, itemLockInfo.Owner, itemLockInfo.Timeout, new Uri($"{TokenScheme}:{itemLockInfo.Token:D}"), itemLockInfo.LockRootUri);
        }

        /// <summary>
        /// The GetTokenFromLockToken.
        /// </summary>
        /// <param name="lockTokenUri">The lockTokenUri<see cref="Uri"/>.</param>
        /// <returns>The <see cref="Guid?"/>.</returns>
        private static Guid? GetTokenFromLockToken(Uri lockTokenUri)
        {
            // We should always use opaquetokens
            if (lockTokenUri.Scheme != TokenScheme)
                return null;

            // Parse the token
            if (!Guid.TryParse(lockTokenUri.LocalPath, out var lockToken))
                return null;

            // Return the token
            return lockToken;
        }
    }
}
