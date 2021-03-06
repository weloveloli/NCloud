// -----------------------------------------------------------------------
// <copyright file="AliyunRefreshTokenCronJob.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NCloud.Core;
    using NCloud.FileProviders.Support.Logger;

    /// <summary>
    /// AliyunRefreshTokenCronJob
    /// </summary>
    public class AliyunRefreshTokenCronJob : CronJobService
    {
        private readonly ILogger<AliyunRefreshTokenCronJob> logger;

        public static List<AliyunDriveFileProvider> AliyunDriveFileProviders { get; set; } = new List<AliyunDriveFileProvider>();

        /// <summary>
        /// 
        /// </summary>
        public AliyunRefreshTokenCronJob() : base(@"0 0 */2 * *", TimeZoneInfo.Local)
        {
            this.logger = ApplicationLogging.CreateLogger<AliyunRefreshTokenCronJob>();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var providers in AliyunDriveFileProviders)
            {
                logger.LogInformation("Refresh AliyunDriveFileProvider: {providerKey}", providers.Key);
                providers.Refresh();
            }

            return base.StartAsync(cancellationToken);
        }
    }
}
