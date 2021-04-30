// -----------------------------------------------------------------------
// <copyright file="DriveFactory.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Core
{
    /// <summary>
    /// Defines the <see cref="IDriveFactory" />.
    /// </summary>
    public interface IDriveFactory : IDrive
    {
        /// <summary>
        /// The EnsureDrive.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        /// <param name="force">The force<see cref="bool"/>.</param>
        /// <returns>The <see cref="bool"/>return false when there is existing drive register with given path.</returns>
        public bool TryEnableDrive(string path, string config, bool force = true);

        /// <summary>
        /// The TryAddRoot.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="force">The force<see cref="bool"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool TryAddRoot(string path, string name, bool force = true);
    }
}
