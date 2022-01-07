// -----------------------------------------------------------------------
// <copyright file="UriResultCollection.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using NWebDav.Server.Helpers;

    /// <summary>
    /// Defines the <see cref="UriResultCollection" />.
    /// </summary>
    internal class UriResultCollection
    {
        /// <summary>
        /// Defines the <see cref="UriResult" />.
        /// </summary>
        private struct UriResult
        {
            /// <summary>
            /// Gets the Uri.
            /// </summary>
            private Uri Uri { get; }

            /// <summary>
            /// Gets the Result.
            /// </summary>
            private DavStatusCode Result { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref=""/> class.
            /// </summary>
            /// <param name="uri">The uri<see cref="Uri"/>.</param>
            /// <param name="result">The result<see cref="DavStatusCode"/>.</param>
            public UriResult(Uri uri, DavStatusCode result)
            {
                Uri = uri;
                Result = result;
            }

            /// <summary>
            /// The GetXmlResponse.
            /// </summary>
            /// <returns>The <see cref="XElement"/>.</returns>
            public XElement GetXmlResponse()
            {
                var statusText = $"HTTP/1.1 {(int)Result} {DavStatusCodeHelper.GetStatusDescription(Result)}";
                return new XElement(WebDavNamespaces.DavNs + "response",
                    new XElement(WebDavNamespaces.DavNs + "href", UriHelper.ToEncodedString(Uri)),
                    new XElement(WebDavNamespaces.DavNs + "status", statusText));
            }
        }

        /// <summary>
        /// Defines the _results.
        /// </summary>
        private readonly IList<UriResult> _results = new List<UriResult>();

        /// <summary>
        /// Gets a value indicating whether HasItems.
        /// </summary>
        public bool HasItems => _results.Any();

        /// <summary>
        /// The AddResult.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="result">The result<see cref="DavStatusCode"/>.</param>
        public void AddResult(Uri uri, DavStatusCode result)
        {
            _results.Add(new UriResult(uri, result));
        }

        /// <summary>
        /// The GetXmlMultiStatus.
        /// </summary>
        /// <returns>The <see cref="XElement"/>.</returns>
        public XElement GetXmlMultiStatus()
        {
            var xMultiStatus = new XElement(WebDavNamespaces.DavNs + "multistatus");
            foreach (var result in _results)
                xMultiStatus.Add(result.GetXmlResponse());
            return xMultiStatus;
        }
    }
}
