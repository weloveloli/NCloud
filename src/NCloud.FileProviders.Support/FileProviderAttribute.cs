// -----------------------------------------------------------------------
// <copyright file="FileProviderAttributes.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using System;

    /// <summary>
    /// Defines the <see cref="FileProviderAttribute" />.
    /// </summary>
    public class FileProviderAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Protocol.
        /// </summary>
        public string Protocol { get; set; }
    }
}
