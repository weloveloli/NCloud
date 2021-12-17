// -----------------------------------------------------------------------
// <copyright file="HttpContextExtensions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.App.Extentions
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Defines the <see cref="HttpContextExtensions" />.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Defines the EmptyRouteData.
        /// </summary>
        private static readonly RouteData EmptyRouteData = new RouteData();

        /// <summary>
        /// Defines the EmptyActionDescriptor.
        /// </summary>
        private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();

        /// <summary>
        /// The WriteResultAsync.
        /// </summary>
        /// <typeparam name="TResult">.</typeparam>
        /// <param name="context">The context<see cref="HttpContext"/>.</param>
        /// <param name="result">The result.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public static Task WriteResultAsync<TResult>(this HttpContext context, TResult result)
            where TResult : IActionResult
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var executor = context.RequestServices.GetService<IActionResultExecutor<TResult>>();

            if (executor == null)
            {
                throw new InvalidOperationException($"No result executor for '{typeof(TResult).FullName}' has been registered.");
            }

            var routeData = context.GetRouteData() ?? EmptyRouteData;

            var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

            return executor.ExecuteAsync(actionContext, result);
        }
    }
}
