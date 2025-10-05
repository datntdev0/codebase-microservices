using datntdev.Microservices.Common.Repository;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Srv.Admin.Web.App
{
    public class SrvAdminDbContext(DbContextOptions<SrvAdminDbContext> options) 
        : DbContext(options), IDocumentDbContext
    {
    }
}
