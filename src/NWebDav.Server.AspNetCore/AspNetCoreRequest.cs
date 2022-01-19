﻿// -----------------------------------------------------------------------
// <copyright file="AspNetCoreRequest.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Microsoft.AspNetCore.Http;
    using NWebDav.Server.Http;

    public partial class AspNetCoreContext
    {
        private class AspNetCoreRequest : IHttpRequest
        {
            private readonly HttpRequest _request;

            internal AspNetCoreRequest(HttpRequest request)
            {
                _request = request;
            }

            public string HttpMethod => _request.Method;
            public Uri Url => new Uri($"{_request.Scheme}://{_request.Host}{_request.Path}");
            public string RemoteEndPoint => new IPEndPoint(_request.HttpContext.Connection.RemoteIpAddress, _request.HttpContext.Connection.RemotePort).ToString();
            public IEnumerable<string> Headers => _request.Headers.Keys;
            public string GetHeaderValue(string header) => _request.Headers[header].FirstOrDefault();
            public Stream Stream => _request.Body;
        }
    }
}
