﻿namespace datntdev.Abp.EntityFramework
{
    public interface IShouldInitializeDcontext
    {
        void Initialize(AbpEfDbContextInitializationContext initializationContext);
    }
}