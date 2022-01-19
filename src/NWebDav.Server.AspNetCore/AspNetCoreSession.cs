// -----------------------------------------------------------------------
// <copyright file="AspNetCoreSession.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.AspNetCore
{
    using System.Security.Claims;
    using System.Security.Principal;
    using NWebDav.Server.Http;

    public partial class AspNetCoreContext
    {
        private class AspNetCoreSession : IHttpSession
        {
            internal AspNetCoreSession(ClaimsPrincipal principal)
            {
                Principal = principal;
            }

            public IPrincipal Principal { get; }
        }
    }
}
