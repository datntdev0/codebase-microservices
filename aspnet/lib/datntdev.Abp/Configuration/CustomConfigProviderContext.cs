﻿using datntdev.Abp.Dependency;

namespace datntdev.Abp.Configuration
{
    public class CustomConfigProviderContext
    {
        public IScopedIocResolver IocResolver { get; }

        public CustomConfigProviderContext(IScopedIocResolver iocResolver)
        {
            IocResolver = iocResolver;
        }
    }
}