// -----------------------------------------------------------------------
// <copyright file="FileInfoMiddleware.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.App.Middeleware
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using NCloud.Core;

    /// <summary>
    /// Defines the <see cref="FileInfoMiddleware" />.
    /// </summary>
    public class FileInfoMiddleware
    {
        /// <summary>
        /// Defines the next.
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        /// Defines the driveFactory.
        /// </summary>
        private readonly IDriveFactory driveFactory;

        /// <summary>
        /// Defines the prefix.
        /// </summary>
        private const string prefix = "/api/fileinfo";

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfoMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next<see cref="RequestDelegate"/>.</param>
        /// <param name="driveFactory">The driveFactory<see cref="IDriveFactory"/>.</param>
        public FileInfoMiddleware(RequestDelegate next, IDriveFactory driveFactory)
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
                var path = context.Request.Path.Value;
                path = path.Substring(prefix.Length);
                path = string.IsNullOrEmpty(path) ? "/" : path;
                var fileInfo = await driveFactory.GetFileInfosByPathAsync(path);
                await context.Response.WriteAsJsonAsync(fileInfo);
            }
            await this.next.Invoke(context);
        }
    }
}
