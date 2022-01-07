// -----------------------------------------------------------------------
// <copyright file="BasicHttpContext.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.HttpListener
{
    using System;
    using System.Net;
    using System.Security;
    using System.Security.Principal;
    using NWebDav.Server.Http;

    /// <summary>
    /// Defines the <see cref="HttpBasicContext" />.
    /// </summary>
    public class HttpBasicContext : HttpBaseContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpBasicContext"/> class.
        /// </summary>
        /// <param name="httpListenerContext">The httpListenerContext<see cref="HttpListenerContext"/>.</param>
        /// <param name="getPrincipal">The getPrincipal<see cref="Func{HttpListenerBasicIdentity, IPrincipal}"/>.</param>
        public HttpBasicContext(HttpListenerContext httpListenerContext, Func<HttpListenerBasicIdentity, IPrincipal> getPrincipal) : base(httpListenerContext.Request, httpListenerContext.Response)
        {
            // Obtain the basic identity
            var basicIdentity = httpListenerContext.User?.Identity as HttpListenerBasicIdentity;

            // Resolve to a principal
            var principal = getPrincipal(basicIdentity);

            // Create the session
            Session = new HttpSession(principal);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpBasicContext"/> class.
        /// </summary>
        /// <param name="httpListenerContext">The httpListenerContext<see cref="HttpListenerContext"/>.</param>
        /// <param name="checkIdentity">The checkIdentity<see cref="Func{HttpListenerBasicIdentity, bool}"/>.</param>
        public HttpBasicContext(HttpListenerContext httpListenerContext, Func<HttpListenerBasicIdentity, bool> checkIdentity) : base(httpListenerContext.Request, httpListenerContext.Response)
        {
            // Obtain the basic identity
            var basicIdentity = httpListenerContext.User?.Identity as HttpListenerBasicIdentity;
            if (!checkIdentity(basicIdentity))
                throw new SecurityException("Basic authorization failed.");

            // Create the session
            Session = new HttpSession(httpListenerContext.User);
        }

        /// <summary>
        /// Gets the Session.
        /// </summary>
        public override IHttpSession Session { get; }
    }
}
