﻿using System;
using datntdev.Abp.Configuration.Startup;

namespace datntdev.Abp.BackgroundJobs
{
    public class BackgroundJobConfiguration : IBackgroundJobConfiguration
    {
        public static int DefaultMaxWaitingJobToProcessPerPeriod = 1000;
        
        public bool IsJobExecutionEnabled { get; set; }

        [Obsolete("Use UserTokenExpirationPeriod instead.")]
        public int? CleanUserTokenPeriod { get; set; }

        public TimeSpan? UserTokenExpirationPeriod { get; set; }

        public int MaxWaitingJobToProcessPerPeriod { get; set; } = DefaultMaxWaitingJobToProcessPerPeriod;
        
        public IAbpStartupConfiguration AbpConfiguration { get; private set; }

        public BackgroundJobConfiguration(IAbpStartupConfiguration abpConfiguration)
        {
            AbpConfiguration = abpConfiguration;

            IsJobExecutionEnabled = true;
        }
    }
}
