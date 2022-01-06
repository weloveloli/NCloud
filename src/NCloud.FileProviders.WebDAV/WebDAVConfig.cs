// -----------------------------------------------------------------------
// <copyright file="WebDAVConfig.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.WebDAV
{
    using NCloud.FileProviders.Abstractions;

    /// <summary>
    /// Defines the <see cref="WebDAVConfig" />.
    /// </summary>
    public class WebDAVConfig : BaseProviderConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebDAVConfig"/> class.
        /// </summary>
        public WebDAVConfig() : base()
        {
        }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the BasePath.
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets or sets the User.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets the Type.
        /// </summary>
        public override string Type => "webdav";
    }
}
