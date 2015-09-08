namespace MeetyChat.Tests.IntegrationTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;

    [TestClass]
    public class UserControllerTests : BaseIntegrationTest
    {
        [TestMethod]
        public void LoginShouldReturnAuthTokenWith200Ok()
        {
            var httpResponse = this.Login(SeededUserUsername, SeededUserPassword);
            var responseValues = httpResponse.Content.ReadAsAsync<UserSessionModel>().Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.IsNotNull(responseValues.Access_Token);
        }

        [TestMethod]
        public void RegisterWithValidDataShouldReturnAccessTokenWith200Ok()
        {
            var loginData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", RegisterTestUsername),
                new KeyValuePair<string, string>("password", "fikretstoraro"),
                new KeyValuePair<string, string>("confirmPassword", "fikretstoraro"),
                new KeyValuePair<string, string>("name", "Fikret Storaro"),
                new KeyValuePair<string, string>("email", "nqmam@kraka.tr"),
                new KeyValuePair<string, string>("gender", 2.ToString())
            });

            var httpResponse = this.httpClient.PostAsync(ApiEndpoints.UserRegister, loginData).Result;

            var responseValues = httpResponse.Content.ReadAsAsync<UserSessionModel>().Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.IsNotNull(responseValues.Access_Token);
        }

        [TestMethod]
        public void LogoutShouldReturn200Ok()
        {
            this.Login(SeededUserUsername, SeededUserPassword);

            var logoutResponse = this.httpClient.PostAsync(ApiEndpoints.UserLogout, null).Result;

            Assert.AreEqual(HttpStatusCode.OK, logoutResponse.StatusCode);
        }
    }
}
