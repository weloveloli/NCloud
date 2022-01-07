// -----------------------------------------------------------------------
// <copyright file="HttpResponse.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.HttpListener
{
    using System.IO;
    using System.Net;
    using NWebDav.Server.Http;

    /// <summary>
    /// Defines the <see cref="HttpResponse" />.
    /// </summary>
    public class HttpResponse : IHttpResponse
    {
        /// <summary>
        /// Defines the _response.
        /// </summary>
        private readonly HttpListenerResponse _response;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponse"/> class.
        /// </summary>
        /// <param name="response">The response<see cref="HttpListenerResponse"/>.</param>
        internal HttpResponse(HttpListenerResponse response)
        {
            _response = response;
        }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        public int Status { get => _response.StatusCode; set => _response.StatusCode = value; }

        /// <summary>
        /// Gets or sets the StatusDescription.
        /// </summary>
        public string StatusDescription { get => _response.StatusDescription; set => _response.StatusDescription = value; }

        /// <summary>
        /// The SetHeaderValue.
        /// </summary>
        /// <param name="header">The header<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="string"/>.</param>
        public void SetHeaderValue(string header, string value)
        {
            switch (header)
            {
                case "Content-Length":
                    _response.ContentLength64 = long.Parse(value);
                    break;

                case "Content-Type":
                    _response.ContentType = value;
                    break;

                default:
                    _response.Headers[header] = value;
                    break;
            }
        }

        /// <summary>
        /// Gets the Stream.
        /// </summary>
        public Stream Stream => _response.OutputStream;
    }
}
