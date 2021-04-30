// -----------------------------------------------------------------------
// <copyright file="ISystemHelper.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Core
{
    /// <summary>
    /// Defines the <see cref="ISystemHelper" />.
    /// </summary>
    public interface ISystemHelper
    {
        /// <summary>
        /// The DecodeBase64.
        /// </summary>
        /// <param name="base64">The base64<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string DecodeBase64(string base64);

        /// <summary>
        /// The EncodeBase64.
        /// </summary>
        /// <param name="base64">The base64<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string EncodeBase64(string base64);

        /// <summary>
        /// The NormalizePath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public string NormalizePath(string path);

        /// <summary>
        /// The DenormalizePath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public string DenormalizePath(string path);
    }
}
