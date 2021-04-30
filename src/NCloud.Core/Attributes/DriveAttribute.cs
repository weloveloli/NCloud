// -----------------------------------------------------------------------
// <copyright file="DriveAttribute.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Core.Attributes
{
    using System;

    /// <summary>
    /// Defines the <see cref="DriveAttribute" />.
    /// </summary>
    public class DriveAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public string Protocol{ get; set; }
    }
}
