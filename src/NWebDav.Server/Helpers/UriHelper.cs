// -----------------------------------------------------------------------
// <copyright file="UriHelper.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Helpers
{
    using System;

    /// <summary>
    /// Defines the <see cref="UriHelper" />.
    /// </summary>
    public static class UriHelper
    {
        /// <summary>
        /// The Combine.
        /// </summary>
        /// <param name="baseUri">The baseUri<see cref="Uri"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="Uri"/>.</returns>
        public static Uri Combine(Uri baseUri, string path)
        {
            var uriText = baseUri.OriginalString;
            if (uriText.EndsWith("/"))
                uriText = uriText.Substring(0, uriText.Length - 1);
            return new Uri($"{uriText}/{path}", UriKind.Absolute);
        }

        /// <summary>
        /// The ToEncodedString.
        /// </summary>
        /// <param name="entryUri">The entryUri<see cref="Uri"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToEncodedString(Uri entryUri)
        {
            return entryUri
                .AbsolutePath
                .Replace("#", "%23")
                .Replace("[", "%5B")
                .Replace("]", "%5D");
        }

        /// <summary>
        /// The GetDecodedPath.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetDecodedPath(Uri uri)
        {
            return uri.LocalPath + Uri.UnescapeDataString(uri.Fragment);
        }
    }
}
