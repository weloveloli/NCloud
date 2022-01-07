// -----------------------------------------------------------------------
// <copyright file="DavStatusCodeAttribute.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Helpers
{
    using System;

    /// <summary>
    /// Defines the <see cref="DavStatusCodeAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class DavStatusCodeAttribute : Attribute
    {
        /// <summary>
        /// Gets the Description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DavStatusCodeAttribute"/> class.
        /// </summary>
        /// <param name="description">The description<see cref="string"/>.</param>
        public DavStatusCodeAttribute(string description)
        {
            Description = description;
        }
    }
}
