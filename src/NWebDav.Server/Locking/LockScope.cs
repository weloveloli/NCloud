// -----------------------------------------------------------------------
// <copyright file="LockScope.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Locking
{
    using System.Xml.Serialization;

    /// <summary>
    /// Defines the LockScope.
    /// </summary>
    public enum LockScope
    {
        /// <summary>
        /// Defines the Exclusive.
        /// </summary>
        [XmlEnum("exclusive")]
        Exclusive,

        /// <summary>
        /// Defines the Shared.
        /// </summary>
        [XmlEnum("shared")]
        Shared
    }
}
