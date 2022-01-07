// -----------------------------------------------------------------------
// <copyright file="HttpContext.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.HttpListener
{
    using System.Net;
    using NWebDav.Server.Http;

    /// <summary>
    /// Defines the <see cref="HttpContext" />.
    /// </summary>
    public class HttpContext : HttpBaseContext
    {
        /// <summary>
        /// Defines the s_nullSession.
        /// </summary>
        private static readonly IHttpSession s_nullSession = new HttpSession(null);

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpContext"/> class.
        /// </summary>
        /// <param name="httpListenerContext">The httpListenerContext<see cref="HttpListenerContext"/>.</param>
        public HttpContext(HttpListenerContext httpListenerContext) : base(httpListenerContext.Request, httpListenerContext.Response)
        {
        }

        /// <summary>
        /// Gets the Session.
        /// </summary>
        public override IHttpSession Session => s_nullSession;
    }
}
