// -----------------------------------------------------------------------
// <copyright file="AspNetCoreResponse.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.AspNetCore
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.Extensions.FileProviders;
    using NWebDav.Server.Http;

    public partial class AspNetCoreContext
    {
        private class AspNetCoreResponse : IHttpResponse
        {
            private const int StreamCopyBufferSize = 64 * 1024;
            private readonly HttpResponse _response;

            internal AspNetCoreResponse(HttpResponse response)
            {
                _response = response;
            }

            public int Status {
                get => _response.StatusCode;
                set => _response.StatusCode = value;
            }

            // Status Description isn't send to the requester
            public string StatusDescription {
                get;
                set;
            }

            public void SetHeaderValue(string header, string value)
            {
                switch (header.ToLowerInvariant())
                {
                    case "content-length":
                        _response.ContentLength = long.Parse(value);
                        break;

                    case "content-type":
                        _response.ContentType = value;
                        break;

                    default:
                        _response.Headers[header] = value;
                        break;
                }
            }

            public async Task SendFileAsync(Stream stream, long offset, long? count, CancellationToken cancellationToken = default)
            {
                if(cancellationToken == default)
                {
                    cancellationToken = _response.HttpContext.RequestAborted;
                }

                var useRequestAborted = !cancellationToken.CanBeCanceled;
                var localCancel = useRequestAborted ? _response.HttpContext.RequestAborted : cancellationToken;

                try
                {
                    localCancel.ThrowIfCancellationRequested();
                    if (offset > 0)
                    {
                        stream.Seek(offset, SeekOrigin.Begin);
                    }
                   await StreamCopyOperation.CopyToAsync(stream, _response.Body, count, StreamCopyBufferSize, localCancel);
                }
                catch (OperationCanceledException) when (useRequestAborted) { }
            }
            public Stream Stream => _response.Body;
        }
    }
}
