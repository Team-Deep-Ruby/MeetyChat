namespace MeetyChat.Tests.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Hosting;
    using System.Web.Http.Routing;
    using System.Web.Script.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Models;
    using Services.Controllers;
    using Services.Models;
    using Services.Models.Users;

    [TestClass]
    public class ProfileControllerTests
    {
        private ProfileController controller;
        private JavaScriptSerializer serializer;
        private MeetyChatDataMock unitOfWorkMock;

        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWorkMock = new MeetyChatDataMock();

            this.controller = new ProfileController(this.unitOfWorkMock, UserIdProviderMock.GetUserIdProvider().Object);
            this.serializer = new JavaScriptSerializer();
            this.SetupController();
        }

        [TestMethod]
        public void GetProfileInfoShouldReturnProfileInfo()
        {
            var httpResponse = this.controller
                .GetProfileInfo()
                .ExecuteAsync(new CancellationToken()).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var serverResponse = httpResponse.Content
                .ReadAsStringAsync().Result;

            var profileInfo =
                this.serializer
                .Deserialize<ProfileViewModel>(serverResponse);

            var expectedUser = this.unitOfWorkMock
                .Users.All()
                .First();

            Assert.AreEqual(expectedUser.Id, profileInfo.Id);

        }

        [TestMethod]
        public void EditUserShouldEditUser()
        {
            var editUserInfo = new EditUserBindingModel()
            {
                Name = "edited",
                Email = "edited@com.com",
                Gender = Gender.Male
            };

            var initialName = this.unitOfWorkMock
                .Users.All()
                .Select(u => u.Name)
                .First();

            var httpResponse = 
                this.controller.EditProfileInfo(editUserInfo)
                .ExecuteAsync(new CancellationToken()).Result;

            Assert.AreEqual(HttpStatusCode.OK, 
                httpResponse.StatusCode);

            var newName = this.unitOfWorkMock
                .Users.All()
                .Select(u => u.Name)
                .First();

            Assert.AreEqual("edited", newName);
        }

        private void SetupController()
        {

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "messages" } });

            this.controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            this.controller.Request = request;
            this.controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }
    }
}