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
    using NCloud.Core;

    /// <summary>
    /// AliyunRefreshTokenCronJob
    /// </summary>
    public class AliyunRefreshTokenCronJob : CronJobService
    {
        public static List<AliyunDriveFileProvider> AliyunDriveFileProviders { get; set; } = new List<AliyunDriveFileProvider>();


        public AliyunRefreshTokenCronJob() : base(@"0 0 */2 * *", TimeZoneInfo.Local)
        {
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var providers in AliyunDriveFileProviders)
            {
                providers.Refresh();
            }

            return base.StartAsync(cancellationToken);
        }
    }
}
