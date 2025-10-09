namespace datntdev.Microservices.Srv.Identity.Tests
{
    [TestClass]
    public sealed class GetHealthCheckTests : TestBase
    {
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
