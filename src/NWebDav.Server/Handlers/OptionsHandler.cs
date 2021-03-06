// -----------------------------------------------------------------------
// <copyright file="OptionsHandler.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Handlers
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using NWebDav.Server.Helpers;
    using NWebDav.Server.Http;
    using NWebDav.Server.Stores;

    /// <summary>
    /// Implementation of the OPTIONS method.
    /// </summary>
    public class OptionsHandler : IRequestHandler
    {
        /// <summary>
        /// Handle a OPTIONS request.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="store">The store<see cref="IStore"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public Task<bool> HandleRequestAsync(IHttpContext httpContext, IStore store)
        {
            // Obtain response
            var response = httpContext.Response;

            // We're a DAV class 1 and 2 compatible server
            response.SetHeaderValue("Dav", "1,2,3,sabredav-partialupdate");
            response.SetHeaderValue("MS-Author-Via", "DAV");

            // Set the Allow/Public headers
            response.SetHeaderValue("allow", "OPTIONS,PROPFIND,COPY,LOCK,UNLOCK");
            response.SetHeaderValue("date", DateTime.Now.ToString("r", CultureInfo.GetCultureInfo("en-US")));

            // Finished
            response.SetStatus(DavStatusCode.Ok);
            return Task.FromResult(true);
        }
    }
}
