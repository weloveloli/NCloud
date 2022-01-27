// -----------------------------------------------------------------------
// <copyright file="AliyunDriveStartUp.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive
{
    using Microsoft.Extensions.DependencyInjection;
    using NCloud.Core;

    public class AliyunDriveStartUp : INCloudStartUp
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services">set up refresh token cron job</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCronJob<AliyunRefreshTokenCronJob>();
        }
    }
}
