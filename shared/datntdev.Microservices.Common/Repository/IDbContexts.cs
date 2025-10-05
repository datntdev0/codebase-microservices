namespace datntdev.Microservices.Common.Repository
{
    public interface IDbContext
    {
    }

    public interface IRelationalDbContext : IDbContext
    {
    }

    public interface IDocumentDbContext : IDbContext
    {
    }
}
