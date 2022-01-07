// -----------------------------------------------------------------------
// <copyright file="DavStatusCodeHelper.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Helpers
{
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Helper methods for the <see cref="DavStatusCode"/> enumeration.
    /// </summary>
    public static class DavStatusCodeHelper
    {
        /// <summary>
        /// Obtain the human-readable status description for the specified
        /// <see cref="DavStatusCode"/>.
        /// </summary>
        /// <param name="davStatusCode">The davStatusCode<see cref="DavStatusCode"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetStatusDescription(this DavStatusCode davStatusCode)
        {
            // Obtain the member information
            var memberInfo = typeof(DavStatusCode).GetMember(davStatusCode.ToString()).FirstOrDefault();
            if (memberInfo == null)
                return null;

            var davStatusCodeAttribute = memberInfo.GetCustomAttribute<DavStatusCodeAttribute>();
            return davStatusCodeAttribute?.Description;
        }
    }
}
