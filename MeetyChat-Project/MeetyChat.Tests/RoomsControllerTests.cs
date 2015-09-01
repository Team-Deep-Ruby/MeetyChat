namespace MeetyChat.Tests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Hosting;
    using System.Web.Http.Routing;
    using System.Web.Script.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.Controllers;
    using Services.Models.Rooms;

    [TestClass]
    public class RoomsControllerTests
    {
        private RoomsController controller;
        private JavaScriptSerializer serializer;

        [TestInitialize]
        public void Initialize()
        {
            this.controller = new RoomsController(Mocks.GetUnitOfWorkMock().Object, Mocks.GetUserIdProvider().Object);
            this.serializer = new JavaScriptSerializer();
            this.SetupController();
        }

        [TestMethod]
        public void TestGettingAllRoomsShouldReturnAllRooms()
        {
            var httpResponse = this.controller.GetAllRooms().ExecuteAsync(new CancellationToken()).Result;

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;
            var rooms = this.serializer.Deserialize<IList<RoomViewModel>>(serverResponseJson);
            var responseRoomsJson = this.serializer.Serialize(rooms);

            var expectedResult = GetExpectedRoomsResult();
            var expectedJson = this.serializer.Serialize(expectedResult);

            Assert.AreEqual(expectedJson, responseRoomsJson);
        }

        private void SetupController()
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "rooms" } });

            this.controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            this.controller.Request = request;
            this.controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        private IList<RoomViewModel> GetExpectedRoomsResult()
        {
            return new List<RoomViewModel>()
            {
                new RoomViewModel()
                {
                    Id = 1,
                    Name = "room 1",
                    MembersCount = 1,
                },
                new RoomViewModel()
                {
                    Id = 2,
                    Name = "room 2",
                    MembersCount = 1,
                },
                new RoomViewModel()
                {
                    Id = 3,
                    Name = "room 3",
                    MembersCount = 1,
                },
            };
        } 
    }
}
