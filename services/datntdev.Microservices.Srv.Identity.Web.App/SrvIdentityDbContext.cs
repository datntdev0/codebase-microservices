using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Srv.Identity.Web.App
{
    public class SrvIdentityDbContext(DbContextOptions<SrvIdentityDbContext> options) : DbContext(options)
    {
    }
}
