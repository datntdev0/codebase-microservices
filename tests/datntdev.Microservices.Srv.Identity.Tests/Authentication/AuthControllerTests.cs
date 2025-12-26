using datntdev.Microservices.Common;

namespace datntdev.Microservices.Srv.Identity.Authentication
{
    [TestClass]
    public sealed class AuthControllerTests : SrvIdentityTestBase
    {
        private static string _cookies = string.Empty;

        [ClassInitialize]
        public static async Task ClassInit(TestContext context)
        {
            var response = await _client.GetAsync("/");
            _cookies = string.Join("; ", response.Headers.GetValues("Set-Cookie"));
        }

        [TestMethod]
        public async Task GetToken_ClientCredentialsGrant_ReturnsAccessToken()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, Constants.Endpoints.OAuth2Token)
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

        [TestMethod]
        public async Task PostSignInForm_WithoutAntiForgery_ReturnsBadRequest()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, Constants.Endpoints.AuthSignIn)
            {
                Content = new FormUrlEncodedContent(
                [
                    new KeyValuePair<string, string>("_handler", "signin"),
                    new KeyValuePair<string, string>("Model.Email", "admin@datntdev.com"),
                    new KeyValuePair<string, string>("Model.Password", "Admin@123"),
                ]),
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task PostSignInForm_WithAntiforgeryToken_ReturnsRedirect()
        {
            var antiForgeryToken = await GetAntiForgeryToken(Constants.Endpoints.AuthSignIn);

            // Step 3: Post the form with the anti-forgery token
            var request = new HttpRequestMessage(HttpMethod.Post, Constants.Endpoints.AuthSignIn)
            {
                Content = new FormUrlEncodedContent(
                [
                    new KeyValuePair<string, string>("_handler", "signin"),
                    new KeyValuePair<string, string>("Model.Email", "admin@datntdev.com"),
                    new KeyValuePair<string, string>("Model.Password", "Admin@123"),
                    new KeyValuePair<string, string>("__RequestVerificationToken", antiForgeryToken),
                ]),
            };

            // Copy cookies (especially the anti-forgery cookie) from GET to POST
            request.Headers.Add("Cookie", string.Join("; ", _cookies));

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Redirect, response.StatusCode);
        }

        [TestMethod]
        public async Task PostSignInForm_WithWrongPassword_ReturnsErrorPopup()
        {
            var antiForgeryToken = await GetAntiForgeryToken(Constants.Endpoints.AuthSignIn);

            // Step 3: Post the form with the anti-forgery token
            var request = new HttpRequestMessage(HttpMethod.Post, Constants.Endpoints.AuthSignIn)
            {
                Content = new FormUrlEncodedContent(
                [
                    new KeyValuePair<string, string>("_handler", "signin"),
                    new KeyValuePair<string, string>("Model.Email", "admin@datntdev.com"),
                    new KeyValuePair<string, string>("Model.Password", "123Qwe!@#"),
                    new KeyValuePair<string, string>("__RequestVerificationToken", antiForgeryToken),
                ]),
            };

            // Copy cookies (especially the anti-forgery cookie) from GET to POST
            request.Headers.Add("Cookie", string.Join("; ", _cookies));

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(content.Contains("Login Failed"));
        }

        [TestMethod]
        public async Task PostSignUpForm_WithAntiforgeryToken_ReturnsRedirect()
        {
            var antiForgeryToken = await GetAntiForgeryToken(Constants.Endpoints.AuthSignUp);
            var username = $"user{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}@email.com";

            // Step 3: Post the form with the anti-forgery token
            var request = new HttpRequestMessage(HttpMethod.Post, Constants.Endpoints.AuthSignUp)
            {
                Content = new FormUrlEncodedContent(
                [
                    new KeyValuePair<string, string>("_handler", "signup"),
                    new KeyValuePair<string, string>("Model.Email", username),
                    new KeyValuePair<string, string>("Model.Password", "User@12345"),
                    new KeyValuePair<string, string>("Model.ConfirmPassword", "User@12345"),
                    new KeyValuePair<string, string>("Model.FirstName", "User"),
                    new KeyValuePair<string, string>("Model.LastName", "Unit Test"),
                    new KeyValuePair<string, string>("__RequestVerificationToken", antiForgeryToken),
                ]),
            };

            // Copy cookies (especially the anti-forgery cookie) from GET to POST
            request.Headers.Add("Cookie", string.Join("; ", _cookies));

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            var content = await response.Content.ReadAsStringAsync();   
            Assert.AreEqual(System.Net.HttpStatusCode.Redirect, response.StatusCode);
        }

        [TestMethod]
        public async Task PostSignUpForm_WithAntiforgeryToken_ReturnsErrorPopup()
        {
            var antiForgeryToken = await GetAntiForgeryToken(Constants.Endpoints.AuthSignUp);

            // Step 3: Post the form with the anti-forgery token
            var request = new HttpRequestMessage(HttpMethod.Post, Constants.Endpoints.AuthSignUp)
            {
                Content = new FormUrlEncodedContent(
                [
                    new KeyValuePair<string, string>("_handler", "signup"),
                    new KeyValuePair<string, string>("Model.Email", "admin@datntdev.com"),
                    new KeyValuePair<string, string>("Model.Password", "User@12345"),
                    new KeyValuePair<string, string>("Model.ConfirmPassword", "User@12345"),
                    new KeyValuePair<string, string>("Model.FirstName", "User"),
                    new KeyValuePair<string, string>("Model.LastName", "Unit Test"),
                    new KeyValuePair<string, string>("__RequestVerificationToken", antiForgeryToken),
                ]),
            };

            // Copy cookies (especially the anti-forgery cookie) from GET to POST
            request.Headers.Add("Cookie", string.Join("; ", _cookies));

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(content.Contains("Registration Failed"));
        }

        private static async Task<string> GetAntiForgeryToken(string urlPath)
        {
            // Step 1: Get the sign-in page to retrieve the anti-forgery token
            var getResponse = await _client.GetAsync(urlPath);
            getResponse.EnsureSuccessStatusCode();
            var getContent = await getResponse.Content.ReadAsStringAsync();

            // Step 2: Extract the anti-forgery token from the HTML
            // This assumes the token is in a hidden input named "__RequestVerificationToken"
            var tokenMatch = System.Text.RegularExpressions.Regex.Match(
                getContent, @"name=""__RequestVerificationToken"" value=""([^""]+)""");
            Assert.IsTrue(tokenMatch.Success, "Anti-forgery token not found in form.");
            var antiForgeryToken = tokenMatch.Groups[1].Value;
            
            return antiForgeryToken;
        }
    }
}
