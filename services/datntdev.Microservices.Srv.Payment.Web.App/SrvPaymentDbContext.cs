using datntdev.Microservices.Common.Repository;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Srv.Payment.Web.App
{
    public class SrvPaymentDbContext(DbContextOptions<SrvPaymentDbContext> options)
        : DbContext(options), IRelationalDbContext
    {
    }
}
