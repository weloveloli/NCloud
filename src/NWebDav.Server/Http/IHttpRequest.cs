// -----------------------------------------------------------------------
// <copyright file="IHttpRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Http
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// HTTP request message interface.
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// Gets the HTTP method of the request..
        /// </summary>
        string HttpMethod { get; }

        /// <summary>
        /// Gets the URL of the request..
        /// </summary>
        Uri Url { get; }

        /// <summary>
        /// Gets the remote end point of the request..
        /// </summary>
        string RemoteEndPoint { get; }

        /// <summary>
        /// Gets the Headers
        /// Gets all headers of the request..
        /// </summary>
        IEnumerable<string> Headers { get; }

        /// <summary>
        /// Gets the value of a request header.
        /// </summary>
        /// <param name="header">The header<see cref="string"/>.</param>
        /// <returns>The header value.</returns>
        string GetHeaderValue(string header);

        /// <summary>
        /// Gets the HTTP request body stream..
        /// </summary>
        Stream Stream { get; }
    }
}
