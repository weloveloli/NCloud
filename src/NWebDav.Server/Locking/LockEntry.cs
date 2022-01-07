// -----------------------------------------------------------------------
// <copyright file="LockEntry.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Locking
{
    using System.Xml.Linq;
    using NWebDav.Server.Helpers;

    /// <summary>
    /// Defines the <see cref="LockEntry" />.
    /// </summary>
    public struct LockEntry
    {
        /// <summary>
        /// Gets the Scope.
        /// </summary>
        public LockScope Scope { get; }

        /// <summary>
        /// Gets the Type.
        /// </summary>
        public LockType Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref=""/> class.
        /// </summary>
        /// <param name="scope">The scope<see cref="LockScope"/>.</param>
        /// <param name="type">The type<see cref="LockType"/>.</param>
        public LockEntry(LockScope scope, LockType type)
        {
            Scope = scope;
            Type = type;
        }

        /// <summary>
        /// The ToXml.
        /// </summary>
        /// <returns>The <see cref="XElement"/>.</returns>
        public XElement ToXml()
        {
            return new XElement(WebDavNamespaces.DavNs + "lockentry",
                new XElement(WebDavNamespaces.DavNs + "lockscope", new XElement(WebDavNamespaces.DavNs + XmlHelper.GetXmlValue(Scope))),
                new XElement(WebDavNamespaces.DavNs + "locktype", new XElement(WebDavNamespaces.DavNs + XmlHelper.GetXmlValue(Type))));
        }
    }
}
