// -----------------------------------------------------------------------
// <copyright file="WebDavLoggerFactory.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.WebDAV.Logging
{
    using System;
    using NCloud.FileProviders.Support.Logger;
    using NWebDav.Server.Logging;

    /// <summary>
    /// Defines the <see cref="WebDavLoggerFactory" />.
    /// </summary>
    public class WebDavLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// The CreateLogger.
        /// </summary>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <returns>The <see cref="ILogger"/>.</returns>
        public ILogger CreateLogger(Type type)
        {
            var logger = ApplicationLogging.CreateLogger(type);
            return new WebDavLogger(logger);
        }
    }

    /// <summary>
    /// Defines the <see cref="WebDavLogger" />.
    /// </summary>
    public class WebDavLogger : ILogger
    {

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly Microsoft.Extensions.Logging.ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDavLogger"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="Microsoft.Extensions.Logging.ILogger"/>.</param>
        public WebDavLogger(Microsoft.Extensions.Logging.ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// The IsLogEnabled.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="LogLevel"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsLogEnabled(LogLevel logLevel)
        {
            var level = ToLevel(logLevel);
            return this.logger.IsEnabled(level);
        }

        /// <summary>
        /// The Log.
        /// </summary>
        /// <param name="logLevel">The logLevel<see cref="LogLevel"/>.</param>
        /// <param name="messageFunc">The messageFunc<see cref="Func{string}"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        public void Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null)
        {
            var level = ToLevel(logLevel);
            this.logger.Log<string>(level, 0, null, exception, (state, err) => messageFunc.Invoke());
        }

        public static Microsoft.Extensions.Logging.LogLevel ToLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return Microsoft.Extensions.Logging.LogLevel.Debug;
                case LogLevel.Info:
                    return Microsoft.Extensions.Logging.LogLevel.Information;
                case LogLevel.Fatal:
                    return Microsoft.Extensions.Logging.LogLevel.Critical;
                case LogLevel.Error:
                    return Microsoft.Extensions.Logging.LogLevel.Error;
                case LogLevel.Warning:
                    return Microsoft.Extensions.Logging.LogLevel.Warning;
                default:
                    return Microsoft.Extensions.Logging.LogLevel.Information;
            }
        }
    }
}
