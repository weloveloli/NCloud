// -----------------------------------------------------------------------
// <copyright file="NCloudStartUp.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Core
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Defines the <see cref="INCloudStartUp" />.
    /// </summary>
    public interface INCloudStartUp
    {
        /// <summary>
        /// The ConfigureServices.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services);
    }
}
