﻿using System;
using datntdev.Abp.Configuration.Startup;

namespace datntdev.Abp.BackgroundJobs
{
    /// <summary>
    /// Used to configure background job system.
    /// </summary>
    public interface IBackgroundJobConfiguration
    {
        /// <summary>
        /// Used to enable/disable background job execution.
        /// </summary>
        bool IsJobExecutionEnabled { get; set; }

        /// <summary>
        /// Period in milliseconds.
        /// </summary>
        [Obsolete("Use UserTokenExpirationPeriod instead.")]
        int? CleanUserTokenPeriod { get; set; }

        /// <summary>
        /// Period for user token expiration worker.
        /// </summary>
        TimeSpan? UserTokenExpirationPeriod { get; set; }

        
        /// <summary>
        /// Maximum number of waiting jobs to process per period.
        /// </summary>
        int MaxWaitingJobToProcessPerPeriod { get; set; }
        
        /// <summary>
        /// Gets the ABP configuration object.
        /// </summary>
        IAbpStartupConfiguration AbpConfiguration { get; }
    }
}
