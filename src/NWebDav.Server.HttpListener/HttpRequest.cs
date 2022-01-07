// -----------------------------------------------------------------------
// <copyright file="HttpRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.HttpListener
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using NWebDav.Server.Http;

    /// <summary>
    /// Defines the <see cref="HttpRequest" />.
    /// </summary>
    public class HttpRequest : IHttpRequest
    {
        /// <summary>
        /// Defines the _request.
        /// </summary>
        private readonly HttpListenerRequest _request;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class.
        /// </summary>
        /// <param name="request">The request<see cref="HttpListenerRequest"/>.</param>
        internal HttpRequest(HttpListenerRequest request)
        {
            _request = request;
        }

        /// <summary>
        /// Gets the HttpMethod.
        /// </summary>
        public string HttpMethod => _request.HttpMethod;

        /// <summary>
        /// Gets the Url.
        /// </summary>
        public Uri Url => _request.Url;

        /// <summary>
        /// Gets the RemoteEndPoint.
        /// </summary>
        public string RemoteEndPoint => _request.UserHostName;

        /// <summary>
        /// Gets the Headers.
        /// </summary>
        public IEnumerable<string> Headers => _request.Headers.AllKeys;

        /// <summary>
        /// The GetHeaderValue.
        /// </summary>
        /// <param name="header">The header<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetHeaderValue(string header) => _request.Headers[header];

        /// <summary>
        /// Gets the Stream.
        /// </summary>
        public Stream Stream => _request.InputStream;
    }
}
