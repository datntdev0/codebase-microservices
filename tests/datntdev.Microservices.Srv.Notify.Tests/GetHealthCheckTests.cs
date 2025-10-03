using datntdev.Microservices.Srv.Notify.Web.Host;
using Microsoft.AspNetCore.Mvc.Testing;

namespace datntdev.Microservices.Srv.Notify.Tests
{
    [TestClass]
    public sealed class GetHealthCheckTests
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

        [TestMethod]
        public async Task GetHealthChecks_ReturnOkStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Healthy", content);
        }
    }
}
