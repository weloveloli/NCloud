// -----------------------------------------------------------------------
// <copyright file="PhysicalProviderConfig.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Physical
{
    using NCloud.FileProviders.Abstractions;

    /// <summary>
    /// Defines the <see cref="PhysicalProviderConfig" />.
    /// </summary>
    public class PhysicalProviderConfig : BaseProviderConfig
    {
        /// <summary>
        /// Gets or sets the Path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets the Type.
        /// </summary>
        public override string Type => "fs";
    }
}
