using datntdev.Microservices.Srv.Identity.Web.Host;
using Microsoft.AspNetCore.Mvc.Testing;

namespace datntdev.Microservices.Srv.Identity.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        protected readonly static WebApplicationFactory<Program> _factory = new();
        protected static HttpClient _client = default!;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext _)
        {
            _client = _factory.CreateClient(new()
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost"),
            });
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            _factory.Dispose();
        }
    }
}
