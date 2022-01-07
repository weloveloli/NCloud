// -----------------------------------------------------------------------
// <copyright file="IOExceptionHelper.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Helpers
{
    using System.IO;

    /// <summary>
    /// Defines the <see cref="IOExceptionHelper" />.
    /// </summary>
    public static class IOExceptionHelper
    {
        /// <summary>
        /// Defines the ErrorHandleDiskFull.
        /// </summary>
        private const int ErrorHandleDiskFull = 0x27;

        /// <summary>
        /// Defines the ErrorDiskFull.
        /// </summary>
        private const int ErrorDiskFull = 0x70;

        /// <summary>
        /// The IsDiskFull.
        /// </summary>
        /// <param name="ioException">The ioException<see cref="IOException"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsDiskFull(this IOException ioException)
        {
            var win32ErrorCode = ioException.HResult & 0xFFFF;
            return win32ErrorCode == ErrorHandleDiskFull || win32ErrorCode == ErrorDiskFull;
        }
    }
}
