// -----------------------------------------------------------------------
// <copyright file="ApplicationLogging.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support.Logger
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="ApplicationLogging" />.
    /// </summary>
    public class ApplicationLogging
    {
        /// <summary>
        /// Defines the _Factory.
        /// </summary>
        private static ILoggerFactory _Factory = null;

        /// <summary>
        /// Gets or sets the LoggerFactory.
        /// </summary>
        public static ILoggerFactory LoggerFactory {
            get {
                return _Factory;
            }
            set { _Factory = value; }
        }

        /// <summary>
        /// The CreateLogger.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="ILogger"/>.</returns>
        public static ILogger<T> CreateLogger<T>() => LoggerFactory?.CreateLogger<T>() ?? default;

        /// <summary>
        /// The CreateLogger.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="ILogger"/>.</returns>
        public static ILogger CreateLogger(Type t) => LoggerFactory?.CreateLogger(t) ?? default;
    }
}
