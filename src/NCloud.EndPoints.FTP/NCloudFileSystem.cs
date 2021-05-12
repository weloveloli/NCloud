// -----------------------------------------------------------------------
// <copyright file="NCloudFileSystem.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using FubarDev.FtpServer.BackgroundTransfer;
    using FubarDev.FtpServer.FileSystem;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="NCloudFileSystem" />.
    /// </summary>
    public class NCloudFileSystem : IUnixFileSystem
    {
        private readonly IFileProvider fileProvider;

        /// <summary>
        /// Gets a value indicating whether SupportsAppend.
        /// </summary>
        public bool SupportsAppend => false;

        /// <summary>
        /// Gets a value indicating whether SupportsNonEmptyDirectoryDelete.
        /// </summary>
        public bool SupportsNonEmptyDirectoryDelete => false;

        /// <summary>
        /// Gets the FileSystemEntryComparer.
        /// </summary>
        public StringComparer FileSystemEntryComparer => StringComparer.Ordinal;

        /// <summary>
        /// Gets the Root.
        /// </summary>
        public IUnixDirectoryEntry Root => throw new NotImplementedException();

        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudFileSystem"/> class.
        /// </summary>
        public NCloudFileSystem(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        }

        /// <summary>
        /// The AppendAsync.
        /// </summary>
        /// <param name="fileEntry">The fileEntry<see cref="IUnixFileEntry"/>.</param>
        /// <param name="startPosition">The startPosition<see cref="long?"/>.</param>
        /// <param name="data">The data<see cref="Stream"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IBackgroundTransfer}"/>.</returns>
        public Task<IBackgroundTransfer> AppendAsync(IUnixFileEntry fileEntry, long? startPosition, Stream data, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The CreateAsync.
        /// </summary>
        /// <param name="targetDirectory">The targetDirectory<see cref="IUnixDirectoryEntry"/>.</param>
        /// <param name="fileName">The fileName<see cref="string"/>.</param>
        /// <param name="data">The data<see cref="Stream"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IBackgroundTransfer}"/>.</returns>
        public Task<IBackgroundTransfer> CreateAsync(IUnixDirectoryEntry targetDirectory, string fileName, Stream data, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The CreateDirectoryAsync.
        /// </summary>
        /// <param name="targetDirectory">The targetDirectory<see cref="IUnixDirectoryEntry"/>.</param>
        /// <param name="directoryName">The directoryName<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IUnixDirectoryEntry}"/>.</returns>
        public Task<IUnixDirectoryEntry> CreateDirectoryAsync(IUnixDirectoryEntry targetDirectory, string directoryName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The GetEntriesAsync.
        /// </summary>
        /// <param name="directoryEntry">The directoryEntry<see cref="IUnixDirectoryEntry"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IReadOnlyList{IUnixFileSystemEntry}}"/>.</returns>
        public Task<IReadOnlyList<IUnixFileSystemEntry>> GetEntriesAsync(IUnixDirectoryEntry directoryEntry, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The GetEntryByNameAsync.
        /// </summary>
        /// <param name="directoryEntry">The directoryEntry<see cref="IUnixDirectoryEntry"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IUnixFileSystemEntry}"/>.</returns>
        public Task<IUnixFileSystemEntry> GetEntryByNameAsync(IUnixDirectoryEntry directoryEntry, string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The MoveAsync.
        /// </summary>
        /// <param name="parent">The parent<see cref="IUnixDirectoryEntry"/>.</param>
        /// <param name="source">The source<see cref="IUnixFileSystemEntry"/>.</param>
        /// <param name="target">The target<see cref="IUnixDirectoryEntry"/>.</param>
        /// <param name="fileName">The fileName<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IUnixFileSystemEntry}"/>.</returns>
        public Task<IUnixFileSystemEntry> MoveAsync(IUnixDirectoryEntry parent, IUnixFileSystemEntry source, IUnixDirectoryEntry target, string fileName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The OpenReadAsync.
        /// </summary>
        /// <param name="fileEntry">The fileEntry<see cref="IUnixFileEntry"/>.</param>
        /// <param name="startPosition">The startPosition<see cref="long"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public Task<Stream> OpenReadAsync(IUnixFileEntry fileEntry, long startPosition, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The ReplaceAsync.
        /// </summary>
        /// <param name="fileEntry">The fileEntry<see cref="IUnixFileEntry"/>.</param>
        /// <param name="data">The data<see cref="Stream"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IBackgroundTransfer}"/>.</returns>
        public Task<IBackgroundTransfer> ReplaceAsync(IUnixFileEntry fileEntry, Stream data, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The SetMacTimeAsync.
        /// </summary>
        /// <param name="entry">The entry<see cref="IUnixFileSystemEntry"/>.</param>
        /// <param name="modify">The modify<see cref="DateTimeOffset?"/>.</param>
        /// <param name="access">The access<see cref="DateTimeOffset?"/>.</param>
        /// <param name="create">The create<see cref="DateTimeOffset?"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IUnixFileSystemEntry}"/>.</returns>
        public Task<IUnixFileSystemEntry> SetMacTimeAsync(IUnixFileSystemEntry entry, DateTimeOffset? modify, DateTimeOffset? access, DateTimeOffset? create, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The UnlinkAsync.
        /// </summary>
        /// <param name="entry">The entry<see cref="IUnixFileSystemEntry"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task UnlinkAsync(IUnixFileSystemEntry entry, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
