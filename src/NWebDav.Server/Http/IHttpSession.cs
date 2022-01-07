// -----------------------------------------------------------------------
// <copyright file="IHttpSession.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Http
{
    using System.Security.Principal;

    /// <summary>
    /// HTTP session interface.
    /// </summary>
    public interface IHttpSession
    {
        /// <summary>
        /// Gets the principal of the current request..
        /// </summary>
        IPrincipal Principal { get; }
    }
}
