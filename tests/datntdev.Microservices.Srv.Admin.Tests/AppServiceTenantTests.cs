using datntdev.Microservices.Srv.Admin.Web.Host;
using Microsoft.AspNetCore.Mvc.Testing;

namespace datntdev.Microservices.Srv.Admin.Tests
{
    [TestClass]
    public class AppServiceTenantTests
    {
        private static WebApplicationFactory<Program> _factory = default!;
        private static HttpClient _client = default!;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext _)
        {
            _factory = new WebApplicationFactory<Program>();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            _factory.Dispose();
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            _client = _factory.CreateClient();
        }
    }
}
