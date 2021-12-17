// -----------------------------------------------------------------------
// <copyright file="FileSetting.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Virtual
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="FileSetting" />.
    /// </summary>
    public class FileSetting
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Children.
        /// </summary>
        public List<FileSetting> Children { get; set; }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets a value indicating whether HasChildren.
        /// </summary>
        public bool HasChildren => this.Children != null && this.Children.Any();

        /// <summary>
        /// Gets a value indicating whether IsInvalid.
        /// </summary>
        public bool IsInvalid => !HasChildren && string.IsNullOrEmpty(Content) && string.IsNullOrEmpty(Url);
    }
}
