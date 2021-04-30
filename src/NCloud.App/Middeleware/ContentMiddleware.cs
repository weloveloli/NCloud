// -----------------------------------------------------------------------
// <copyright file="ContentMiddleware.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.App.Middeleware
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using NCloud.Core;

    /// <summary>
    /// Defines the <see cref="ContentMiddleware" />.
    /// </summary>
    public class ContentMiddleware
    {
        /// <summary>
        /// Defines the prefix.
        /// </summary>
        private const string prefix = "/api/filecontent";

        /// <summary>
        /// Defines the next.
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        /// Defines the driveFactory.
        /// </summary>
        private readonly IDriveFactory driveFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next<see cref="RequestDelegate"/>.</param>
        /// <param name="driveFactory">The driveFactory<see cref="IDriveFactory"/>.</param>
        public ContentMiddleware(RequestDelegate next, IDriveFactory driveFactory)
        {
            this.next = next;
            this.driveFactory = driveFactory;
        }

        /// <summary>
        /// The Invoke.
        /// </summary>
        /// <param name="context">The context<see cref="HttpContext"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(prefix))
            {
                var noCache = context.Request.Query.ContainsKey("nocache");
                var path = context.Request.Path.Value;
                path = path.Substring(prefix.Length);
                using var stream = await driveFactory.GetFileStreamByPathAsync(path, !noCache);
                await stream.CopyToAsync(context.Response.Body);
            }
            await this.next.Invoke(context);
        }
    }
}
