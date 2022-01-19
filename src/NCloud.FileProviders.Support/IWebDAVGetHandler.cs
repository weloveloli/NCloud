// -----------------------------------------------------------------------
// <copyright file="IWebDAVGetHandler.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.FileProviders;

    public interface IWebDAVGetHandler :IFileInfo
    {
        public Task<bool> HandleWebDAVGetRequest(HttpContext httpContext);
    }
}
