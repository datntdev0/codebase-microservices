namespace datntdev.Microservices.AppHost.Tests
{
[TestClass]
    public class YarpGatewayTests : AppHostTestBase
    {
        [TestMethod]
        public async Task GetGatewayToMicroservices_ReturnOkStatusCode()
        {
            // Arrange
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token;
            var serviceNames = new Dictionary<string, string> 
            {
                { "srv-admin", "Srv.Admin" },
                { "srv-notify", "Srv.Notify" },
                { "srv-payment", "Srv.Payment" },
                { "srv-identity", "Srv.Identity" },
            };

            // Act
            var client = App.CreateHttpClient("gateway");
            var requests = serviceNames.Select(x => new
            {
                service = x.Key, 
                serviceName = x.Value,
                responseTask = client.GetAsync($"{x.Key}/api/home"),
            });
            await Task.WhenAll(requests.Select(r => r.responseTask));

            // Assert
            foreach (var request in requests)
            {
                var response = request.responseTask.Result;
                var responseString = await response.Content.ReadAsStringAsync();
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual($"Hello World from {request.serviceName}", responseString);
            }
        }
    }
}
