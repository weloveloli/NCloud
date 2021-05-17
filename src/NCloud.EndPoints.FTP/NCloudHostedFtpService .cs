// -----------------------------------------------------------------------
// <copyright file="NCloudHostedFtpService .cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.EndPoints.FTP
{
    using System.Threading;
    using System.Threading.Tasks;
    using FubarDev.FtpServer;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="NCloudHostedFtpService" />.
    /// </summary>
    public class NCloudHostedFtpService : IHostedService
    {
        /// <summary>
        /// Defines the _ftpServerHost.
        /// </summary>
        private readonly IFtpServerHost _ftpServerHost;
        private readonly ILogger<NCloudHostedFtpService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NCloudHostedFtpService"/> class.
        /// </summary>
        /// <param name="ftpServerHost">The FTP server host that gets wrapped as a hosted service.</param>
        public NCloudHostedFtpService(IFtpServerHost ftpServerHost, ILogger<NCloudHostedFtpService> logger)
        {
            _ftpServerHost = ftpServerHost;
            this.logger = logger;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Start the FTP server");
            return _ftpServerHost.StartAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stop the FTP server");
            return _ftpServerHost.StopAsync(cancellationToken);
        }
    }
}
