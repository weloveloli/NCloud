// -----------------------------------------------------------------------
// <copyright file="NCloudException.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Core
{
    using System;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="NCloudException" />.
    /// </summary>
    public class NCloudException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudException"/> class.
        /// </summary>
        /// <param name="@enum">The enum<see cref="ResultEnum"/>.</param>
        public NCloudException(ResultEnum @enum)
        {
            this.Error = @enum;
        }

        /// <summary>
        /// Gets the Error.
        /// </summary>
        public ResultEnum? Error { get; private set; }
    }
}
