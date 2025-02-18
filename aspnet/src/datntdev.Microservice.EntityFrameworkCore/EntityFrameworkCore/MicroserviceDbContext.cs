using Abp.Zero.EntityFrameworkCore;
using datntdev.Microservice.Authorization.Roles;
using datntdev.Microservice.Authorization.Users;
using datntdev.Microservice.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservice.EntityFrameworkCore;

public class MicroserviceDbContext : AbpZeroDbContext<Tenant, Role, User, MicroserviceDbContext>
{
    /* Define a DbSet for each entity of the application */

    public MicroserviceDbContext(DbContextOptions<MicroserviceDbContext> options)
        : base(options)
    {
    }
}
