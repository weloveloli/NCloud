// -----------------------------------------------------------------------
// <copyright file="ActiveLock.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Locking
{
    using System;
    using System.Globalization;
    using System.Xml.Linq;
    using NWebDav.Server.Helpers;

    /// <summary>
    /// Defines the <see cref="ActiveLock" />.
    /// </summary>
    public struct ActiveLock
    {
        /// <summary>
        /// Gets the Type.
        /// </summary>
        public LockType Type { get; }

        /// <summary>
        /// Gets the Scope.
        /// </summary>
        public LockScope Scope { get; }

        /// <summary>
        /// Gets the Depth.
        /// </summary>
        public int Depth { get; }

        /// <summary>
        /// Gets the Owner.
        /// </summary>
        public XElement Owner { get; }

        /// <summary>
        /// Gets the Timeout.
        /// </summary>
        public int Timeout { get; }

        /// <summary>
        /// Gets the LockToken.
        /// </summary>
        public Uri LockToken { get; }

        /// <summary>
        /// Gets the LockRoot.
        /// </summary>
        public Uri LockRoot { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref=""/> class.
        /// </summary>
        /// <param name="type">The type<see cref="LockType"/>.</param>
        /// <param name="scope">The scope<see cref="LockScope"/>.</param>
        /// <param name="depth">The depth<see cref="int"/>.</param>
        /// <param name="owner">The owner<see cref="XElement"/>.</param>
        /// <param name="timeout">The timeout<see cref="int"/>.</param>
        /// <param name="lockToken">The lockToken<see cref="Uri"/>.</param>
        /// <param name="lockRoot">The lockRoot<see cref="Uri"/>.</param>
        public ActiveLock(LockType type, LockScope scope, int depth, XElement owner, int timeout, Uri lockToken, Uri lockRoot)
        {
            Type = type;
            Scope = scope;
            Depth = depth;
            Owner = owner;
            Timeout = timeout;
            LockToken = lockToken;
            LockRoot = lockRoot;
        }

        /// <summary>
        /// The ToXml.
        /// </summary>
        /// <returns>The <see cref="XElement"/>.</returns>
        public XElement ToXml()
        {
            return new XElement(WebDavNamespaces.DavNs + "activelock",
                new XElement(WebDavNamespaces.DavNs + "locktype", new XElement(WebDavNamespaces.DavNs + XmlHelper.GetXmlValue(Type))),
                new XElement(WebDavNamespaces.DavNs + "lockscope", new XElement(WebDavNamespaces.DavNs + XmlHelper.GetXmlValue(Scope))),
                new XElement(WebDavNamespaces.DavNs + "depth", Depth == int.MaxValue ? "infinity" : Depth.ToString(CultureInfo.InvariantCulture)),
                new XElement(WebDavNamespaces.DavNs + "owner", Owner),
                new XElement(WebDavNamespaces.DavNs + "timeout", Timeout == -1 ? "Infinite" : "Second-" + Timeout.ToString(CultureInfo.InvariantCulture)),
                new XElement(WebDavNamespaces.DavNs + "locktoken", new XElement(WebDavNamespaces.DavNs + "href", LockToken.AbsoluteUri)),
                new XElement(WebDavNamespaces.DavNs + "lockroot", new XElement(WebDavNamespaces.DavNs + "href", LockRoot.AbsoluteUri)));
        }
    }
}
