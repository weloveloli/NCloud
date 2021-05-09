// -----------------------------------------------------------------------
// <copyright file="EnumerableDirectoryContents.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// Defines the <see cref="EnumerableDirectoryContents" />.
    /// </summary>
    public class EnumerableDirectoryContents : IDirectoryContents
    {
        /// <summary>
        /// Defines the _entries.
        /// </summary>
        private readonly IEnumerable<IFileInfo> _entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerableDirectoryContents"/> class.
        /// </summary>
        /// <param name="entries">The entries<see cref="IEnumerable{IFileInfo}"/>.</param>
        public EnumerableDirectoryContents(IEnumerable<IFileInfo> entries)
        {
            _entries = entries;
        }

        /// <summary>
        /// Gets a value indicating whether Exists.
        /// </summary>
        public bool Exists => true;

        /// <summary>
        /// The GetEnumerator.
        /// </summary>
        /// <returns>The <see cref="IEnumerator{IFileInfo}"/>.</returns>
        public IEnumerator<IFileInfo> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        /// <summary>
        /// The GetEnumerator.
        /// </summary>
        /// <returns>The <see cref="IEnumerator"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entries.GetEnumerator();
        }
    }
}
