// -----------------------------------------------------------------------
// <copyright file="PropertyInfo.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Props
{
    using System.Xml.Linq;

    /// <summary>
    /// Information structure about a property.
    /// </summary>
    public struct PropertyInfo
    {
        /// <summary>
        /// Name of the property.
        /// </summary>
        /// <remarks>
        /// The name of the property contains of a namespace and the actual name.
        /// </remarks>
        public XName Name { get; }

        /// <summary>
        /// Flag indicating whether or not it's expensive to determine the
        /// property's value.
        /// </summary>
        /// <remarks>
        /// Properties should be marked as expensive, when it is expensive (in
        /// terms of CPU cycles, I/O or duration) to determine its value.
        /// Expensive values are skipped when obtaining all property values to
        /// keep the server fast.
        /// </remarks>
        public bool IsExpensive { get; }        // TODO: Don't use the term 'Expensive'

        /// <summary>
        /// Initializes a new property information object.
        /// </summary>
        /// <param name="name">
        /// Name of the property.
        /// </param>
        /// <param name="isExpensive">
        /// Flag indicating whether or not it's expensive to determine the
        /// property's value.
        /// </param>
        public PropertyInfo(XName name, bool isExpensive)
        {
            Name = name;
            IsExpensive = isExpensive;
        }
    }
}
