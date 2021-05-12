// -----------------------------------------------------------------------
// <copyright file="IRemoteFileInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System.IO;
    using System.Net.Http;
    using Microsoft.Extensions.FileProviders;
    using NCloud.FileProviders.Abstractions;

    /// <summary>
    /// Defines the <see cref="IRemoteFileInfo" />.
    /// </summary>
    public class RemoteFileInfo : FileInfoDecorator
    {
        /// <summary>
        /// Defines the remoteUrl.
        /// </summary>
        private readonly string remoteUrl;

        /// <summary>
        /// Defines the client.
        /// </summary>
        private HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteFileInfo"/> class.
        /// </summary>
        /// <param name="remoteUrl">The remoteUrl<see cref="string"/>.</param>
        /// <param name="fileInfo">The fileInfo<see cref="IFileInfo"/>.</param>
        public RemoteFileInfo(string remoteUrl, IFileInfo fileInfo, HttpClient client = null) : base(fileInfo)
        {
            this.remoteUrl = remoteUrl;
            this.client = client;
        }

        /// <summary>
        /// Gets the RemoteUrl.
        /// </summary>
        public string RemoteUrl => remoteUrl;

        /// <summary>
        /// The CreateReadStream.
        /// </summary>
        /// <returns>The <see cref="Stream"/>.</returns>
        public override Stream CreateReadStream()
        {
            var stream = base.CreateReadStream();
            if (stream == null && client != null)
            {
                stream = client.GetStreamAsync(RemoteUrl).Result;
            }
            return stream;
        }
    }
}
