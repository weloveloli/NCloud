// -----------------------------------------------------------------------
// <copyright file="DefaultSystemHelper.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Drivers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using NCloud.Core;

    /// <summary>
    /// Defines the <see cref="DefaultSystemHelper" />.
    /// </summary>
    public class DefaultSystemHelper : ISystemHelper
    {
        /// <summary>
        /// Defines the regex1.
        /// </summary>
        private Regex regex1 = new Regex(@"^\/([^\/]+?)");

        /// <summary>
        /// Defines the regex2.
        /// </summary>
        private Regex regex2 = new Regex(@"\/");

        /// <summary>
        /// Defines the regex3.
        /// </summary>
        private Regex regex3 = new Regex(@"(?<!\:)\\+$");

        /// <summary>
        /// Defines the regex4.
        /// </summary>
        private Regex regex4 = new Regex(@"\\{2,}");

        /// <summary>
        /// Defines the regex5.
        /// </summary>
        private Regex regex5 = new Regex(@"^([a-z])\:");

        /// <summary>
        /// The DecodeBase64.
        /// </summary>
        /// <param name="encodedString">The encodedString<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string DecodeBase64(string encodedString)
        {
            var bytes = Convert.FromBase64String(encodedString);

            var decodedString = Encoding.UTF8.GetString(bytes);

            return decodedString;
        }

        /// <summary>
        /// The DenormalizePath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public string DenormalizePath(string path)
        {
            bool relative = false;
            if (path.StartsWith("./"))
            {
                relative = true;
                path = path.Substring(2);
            }
            if (OperatingSystem.IsWindows())
            {
                path = regex1.Replace(path, @"$1:\\");
                path = regex2.Replace(path, "\\");
                path = regex3.Replace(path, "");
                path = regex4.Replace(path, "\\");
            }
            if (relative)
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), path);
            }
            return path;
        }

        /// <summary>
        /// The NormalizePath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public string NormalizePath(string path)
        {
            if (OperatingSystem.IsWindows())
            {
                path = path.Split("\\").Aggregate((a, b) => a + "/" + b);
                path = regex5.Replace(path, "/$1");
            }
            return path;
        }

        /// <summary>
        /// The EncodeBase64.
        /// </summary>
        /// <param name="originalString">The originalString<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string EncodeBase64(string originalString)
        {
            var bytes = Encoding.UTF8.GetBytes(originalString);
            var encodedString = Convert.ToBase64String(bytes);
            return encodedString;
        }
    }
}
