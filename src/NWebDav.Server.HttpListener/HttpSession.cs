// -----------------------------------------------------------------------
// <copyright file="HttpSession.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.HttpListener
{
    using System.Security.Principal;
    using NWebDav.Server.Http;

    /// <summary>
    /// Defines the <see cref="HttpSession" />.
    /// </summary>
    public class HttpSession : IHttpSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSession"/> class.
        /// </summary>
        /// <param name="principal">The principal<see cref="IPrincipal"/>.</param>
        internal HttpSession(IPrincipal principal)
        {
            Principal = principal;
        }

        /// <summary>
        /// Gets the Principal.
        /// </summary>
        public IPrincipal Principal { get; }
    }
}
