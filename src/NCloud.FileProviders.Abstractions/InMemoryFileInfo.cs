// -----------------------------------------------------------------------
// <copyright file="InMemoryFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="InMemoryFileInfo" />.
    /// </summary>
    public class InMemoryFileInfo : IVirtualPathFileInfo
    {
        /// <summary>
        /// Gets a value indicating whether Exists.
        /// </summary>
        public bool Exists => true;

        /// <summary>
        /// Gets the Length.
        /// </summary>
        public long Length => _fileContent?.Length ?? 0;

        /// <summary>
        /// Gets the PhysicalPath.
        /// </summary>
        public string PhysicalPath => null;

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the LastModified.
        /// </summary>
        public DateTimeOffset LastModified { get; }

        /// <summary>
        /// Gets a value indicating whether IsDirectory.
        /// </summary>
        public virtual bool IsDirectory => false;

        /// <summary>
        /// Defines the _fileContent.
        /// </summary>
        private readonly byte[] _fileContent;

        /// <summary>
        /// Gets the DynamicPath.
        /// </summary>
        public string DynamicPath { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryFileInfo"/> class.
        /// </summary>
        /// <param name="dynamicPath">The dynamicPath<see cref="string"/>.</param>
        /// <param name="fileContent">The fileContent<see cref="byte[]"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        public InMemoryFileInfo(string dynamicPath, byte[] fileContent, string name)
        {
            DynamicPath = dynamicPath;
            Name = name;
            _fileContent = fileContent;
            LastModified = DateTimeOffset.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryFileInfo"/> class.
        /// </summary>
        /// <param name="dynamicPath">The dynamicPath<see cref="string"/>.</param>
        /// <param name="content">The fileContent<see cref="string"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="base64">The base64<see cref="bool"/>.</param>
        public InMemoryFileInfo(string dynamicPath, string content, string name, bool base64)
        {
            DynamicPath = dynamicPath;
            Name = name;
            _fileContent = base64 ? Convert.FromBase64String(content) : Encoding.UTF8.GetBytes(content);
            LastModified = DateTimeOffset.Now;
        }

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream CreateReadStream()
        {
            return new MemoryStream(_fileContent, false);
        }

        /// <summary>
        /// The ReadAsString.
        /// </summary>
        /// <param name="encoding">The encoding<see cref="Encoding"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string ReadAsString()
        {
            return Encoding.UTF8.GetString(_fileContent);
        }

        /// <summary>
        /// The ReadAsString.
        /// </summary>
        /// <param name="encoding">The encoding<see cref="Encoding"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string ReadAsString(Encoding encoding)
        {
            return encoding.GetString(_fileContent);
        }

        /// <summary>
        /// The GetVirtualPath.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetVirtualPath()
        {
            return DynamicPath;
        }
    }
}
