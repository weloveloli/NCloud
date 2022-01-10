// -----------------------------------------------------------------------
// <copyright file="APIException.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Exceptions
{
    using System;
    using System.Collections.ObjectModel;
    using System.Net;

    /// <summary>
    /// Defines the <see cref="APIException" />.
    /// </summary>
    public class APIException : Exception
    {
        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Headers.
        /// </summary>
        public ReadOnlyDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Gets or sets the StatusCode.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the Code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the ResponseContent.
        /// </summary>
        public string ResponseContent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="APIException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public APIException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APIException"/> class.
        /// </summary>
        /// <param name="ex">The ex<see cref="APIException"/>.</param>
        public APIException(APIException ex) : base(ex.Message)
        {
            Url = ex.Url;
            Headers = ex.Headers;
            StatusCode = ex.StatusCode;
            Code = ex.Code;
            ResponseContent = ex.ResponseContent;
        }

        /// <summary>
        /// The ToString.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString() => $"[{Code}] {Message}";
    }
}
