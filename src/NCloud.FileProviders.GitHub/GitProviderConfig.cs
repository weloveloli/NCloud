// -----------------------------------------------------------------------
// <copyright file="GitProviderConfig.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.GitHub
{
    using NCloud.FileProviders.Abstractions;

    /// <summary>
    /// Defines the <see cref="GitProviderConfig" />.
    /// </summary>
    public class GitProviderConfig : BaseProviderConfig
    {
        /// <summary>
        /// Gets or sets the Owner.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets the Project.
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// Gets the Type.
        /// </summary>
        public override string Type => "github";
    }
}
