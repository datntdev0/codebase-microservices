using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Srv.Notify.Web.App
{
    public class SrvNotifyDbContext(DbContextOptions<SrvNotifyDbContext> options) : DbContext(options)
    {
    }
}
