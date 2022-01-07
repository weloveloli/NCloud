// -----------------------------------------------------------------------
// <copyright file="ILogger.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Logging
{
    using System;

    /// <summary>
    /// Interface for logging events for a specific type.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Check if the specified log level is enabled.
        /// </summary>
        /// <param name="logLevel">Log level that should be checked.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool IsLogEnabled(LogLevel logLevel);

        /// <summary>
        /// Log a message and an optional exception with the specified log level.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="LogLevel"/>.</param>
        /// <param name="messageFunc">The messageFunc<see cref="Func{string}"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        void Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null);
    }
}
