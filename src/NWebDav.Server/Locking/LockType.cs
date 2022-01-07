// -----------------------------------------------------------------------
// <copyright file="LockType.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Locking
{
    using System.Xml.Serialization;

    /// <summary>
    /// Defines the LockType.
    /// </summary>
    public enum LockType
    {
        /// <summary>
        /// Defines the Write.
        /// </summary>
        [XmlEnum("write")]
        Write
    }
}
