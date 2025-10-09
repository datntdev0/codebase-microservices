namespace datntdev.Microservices.Srv.Identity.Tests
{
    [TestClass]
    public sealed class GetAuthenticationTests : TestBase
    {
        [TestMethod]
        public async Task GetToken_ClientCredentialsGrant_ReturnsAccessToken()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/connect/token")
            {
                Content = new FormUrlEncodedContent(
                [
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", "datntdev.Microservices.Confidential"),
                    new KeyValuePair<string, string>("client_secret", "datntdev.Microservices.ClientSecret"),
                ])
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(content.Contains("access_token"));
        }
    }
}
