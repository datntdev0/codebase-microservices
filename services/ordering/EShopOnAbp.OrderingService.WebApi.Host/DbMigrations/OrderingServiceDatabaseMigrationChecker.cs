using EShopOnAbp.OrderingService.EntityFrameworkCore;
using System;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Volo.Abp.DistributedLocking;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;

namespace EShopOnAbp.OrderingService.DbMigrations;

public class OrderingServiceDatabaseMigrationChecker : PendingEfCoreMigrationsChecker<OrderingServiceDbContext>
{
    public OrderingServiceDatabaseMigrationChecker(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDistributedEventBus distributedEventBus,
        IAbpDistributedLock abpDistributedLock)
        : base(
            unitOfWorkManager,
            serviceProvider,
            currentTenant,
            distributedEventBus,
            abpDistributedLock,
            OrderingServiceDbProperties.ConnectionStringName)
    {
    }
}