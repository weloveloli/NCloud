// -----------------------------------------------------------------------
// <copyright file="HttpBaseContext.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.HttpListener
{
    using System.Net;
    using System.Threading.Tasks;
    using NWebDav.Server.Http;

    /// <summary>
    /// Defines the <see cref="HttpBaseContext" />.
    /// </summary>
    public abstract class HttpBaseContext : IHttpContext
    {
        /// <summary>
        /// Defines the _response.
        /// </summary>
        private readonly HttpListenerResponse _response;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpBaseContext"/> class.
        /// </summary>
        /// <param name="request">The request<see cref="HttpListenerRequest"/>.</param>
        /// <param name="response">The response<see cref="HttpListenerResponse"/>.</param>
        protected HttpBaseContext(HttpListenerRequest request, HttpListenerResponse response)
        {
            // Assign properties
            Request = new HttpRequest(request);
            Response = new HttpResponse(response);

            // Save response
            _response = response;
        }

        /// <summary>
        /// Gets the Request.
        /// </summary>
        public IHttpRequest Request { get; }

        /// <summary>
        /// Gets the Response.
        /// </summary>
        public IHttpResponse Response { get; }

        /// <summary>
        /// Gets the Session.
        /// </summary>
        public abstract IHttpSession Session { get; }

        /// <summary>
        /// The CloseAsync.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task CloseAsync()
        {
            // Close the response
            _response.Close();

            // Command completed synchronous
            return Task.FromResult(true);
        }
    }
}
