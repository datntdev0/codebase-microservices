namespace datntdev.Microservices.AppHost.Tests;

[TestClass]
public class GetHealthCheckTests : AppHostTestBase
{
    [TestMethod]
    public async Task GetHealthChecks_ReturnOkStatusCode()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token;
        var clientNames = new string[] { "srv-identity", "srv-admin" };
        await Task.WhenAll(clientNames.Select(x
            => App.ResourceNotifications.WaitForResourceHealthyAsync(x, cancellationToken)));

        // Act
        var httpClients = clientNames.Select(x => App.CreateHttpClient(x));
        var responses = await Task.WhenAll(httpClients.Select(client => client.GetAsync("/health")));

        // Assert
        foreach (var response in responses)
        {
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Healthy", await response.Content.ReadAsStringAsync());
        }
    }
}
