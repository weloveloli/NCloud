// -----------------------------------------------------------------------
// <copyright file="ILockingManager.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Locking
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using NWebDav.Server.Stores;

    /// <summary>
    /// Defines the <see cref="LockResult" />.
    /// </summary>
    public struct LockResult
    {
        /// <summary>
        /// Gets the Result.
        /// </summary>
        public DavStatusCode Result { get; }

        /// <summary>
        /// Gets the Lock.
        /// </summary>
        public ActiveLock? Lock { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref=""/> class.
        /// </summary>
        /// <param name="result">The result<see cref="DavStatusCode"/>.</param>
        /// <param name="@lock">The lock<see cref="ActiveLock?"/>.</param>
        public LockResult(DavStatusCode result, ActiveLock? @lock = null)
        {
            Result = result;
            Lock = @lock;
        }
    }

    // TODO: Call the locking methods from the handlers
    /// <summary>
    /// Defines the <see cref="ILockingManager" />.
    /// </summary>
    public interface ILockingManager
    {
        /// <summary>
        /// The Lock.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <param name="lockType">The lockType<see cref="LockType"/>.</param>
        /// <param name="lockScope">The lockScope<see cref="LockScope"/>.</param>
        /// <param name="owner">The owner<see cref="XElement"/>.</param>
        /// <param name="lockRootUri">The lockRootUri<see cref="Uri"/>.</param>
        /// <param name="recursiveLock">The recursiveLock<see cref="bool"/>.</param>
        /// <param name="timeouts">The timeouts<see cref="IEnumerable{int}"/>.</param>
        /// <returns>The <see cref="LockResult"/>.</returns>
        LockResult Lock(IStoreItem item, LockType lockType, LockScope lockScope, XElement owner, Uri lockRootUri, bool recursiveLock, IEnumerable<int> timeouts);

        /// <summary>
        /// The Unlock.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <param name="token">The token<see cref="Uri"/>.</param>
        /// <returns>The <see cref="DavStatusCode"/>.</returns>
        DavStatusCode Unlock(IStoreItem item, Uri token);

        /// <summary>
        /// The RefreshLock.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <param name="recursiveLock">The recursiveLock<see cref="bool"/>.</param>
        /// <param name="timeouts">The timeouts<see cref="IEnumerable{int}"/>.</param>
        /// <param name="lockTokenUri">The lockTokenUri<see cref="Uri"/>.</param>
        /// <returns>The <see cref="LockResult"/>.</returns>
        LockResult RefreshLock(IStoreItem item, bool recursiveLock, IEnumerable<int> timeouts, Uri lockTokenUri);

        /// <summary>
        /// The GetActiveLockInfo.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <returns>The <see cref="IEnumerable{ActiveLock}"/>.</returns>
        IEnumerable<ActiveLock> GetActiveLockInfo(IStoreItem item);

        /// <summary>
        /// The GetSupportedLocks.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <returns>The <see cref="IEnumerable{LockEntry}"/>.</returns>
        IEnumerable<LockEntry> GetSupportedLocks(IStoreItem item);

        /// <summary>
        /// The IsLocked.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool IsLocked(IStoreItem item);

        /// <summary>
        /// The HasLock.
        /// </summary>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <param name="lockToken">The lockToken<see cref="Uri"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool HasLock(IStoreItem item, Uri lockToken);
    }
}
