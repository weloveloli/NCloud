// -----------------------------------------------------------------------
// <copyright file="IHttpContext.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Http
{
    using System.Threading.Tasks;

    /// <summary>
    /// HTTP context interface.
    /// </summary>
    public interface IHttpContext
    {
        /// <summary>
        /// Gets the current HTTP request message..
        /// </summary>
        IHttpRequest Request { get; }

        /// <summary>
        /// Gets the current HTTP response message..
        /// </summary>
        IHttpResponse Response { get; }

        /// <summary>
        /// Gets the session belonging to the current request..
        /// </summary>
        IHttpSession Session { get; }

        /// <summary>
        /// Close the context.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CloseAsync();


        
    }
}
