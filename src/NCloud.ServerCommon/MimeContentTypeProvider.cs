// -----------------------------------------------------------------------
// <copyright file="MimeContentTypeProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.ServerCommon
{
    using HeyRed.Mime;
    using Microsoft.AspNetCore.StaticFiles;

    /// <summary>
    /// Defines the <see cref="MimeContentTypeProvider" />.
    /// </summary>
    public class MimeContentTypeProvider : IContentTypeProvider
    {
        /// <summary>
        /// The TryGetContentType.
        /// </summary>
        /// <param name="subpath">The subpath<see cref="string"/>.</param>
        /// <param name="contentType">The contentType<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool TryGetContentType(string subpath, out string contentType)
        {
            var extension = GetExtension(subpath);
            if (extension == null)
            {
                contentType = null;
                return false;
            }
            contentType = MimeTypesMap.GetMimeType(extension);
            return true;
        }

        /// <summary>
        /// The GetExtension.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string GetExtension(string path)
        {
            // Don't use Path.GetExtension as that may throw an exception if there are
            // invalid characters in the path. Invalid characters should be handled
            // by the FileProviders

            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }
            if (path.EndsWith("/"))
            {
                return null;
            }
            int index = path.LastIndexOf('.');
            if (index < 0)
            {
                return null;
            }

            return path.Substring(index);
        }
    }
}
