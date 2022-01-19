// -----------------------------------------------------------------------
// <copyright file="IHttpResponse.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Http
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;

    /// <summary>
    /// HTTP response message interface.
    /// </summary>
    public interface IHttpResponse
    {
        /// <summary>
        /// Gets or sets the HTTP status code of the response..
        /// </summary>
        int Status { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status description of the response..
        /// </summary>
        string StatusDescription { get; set; }

        /// <summary>
        /// Sets a header to a specific value.
        /// </summary>
        /// <param name="header">Name of the header.</param>
        /// <param name="value">Value of the header.</param>
        void SetHeaderValue(string header, string value);

        /// <summary>
        /// Gets the stream that represents the response body..
        /// </summary>
        Stream Stream { get; }


        Task SendFileAsync(Stream stream, long offset, long? count, CancellationToken cancellationToken = default(CancellationToken));
    }
}
