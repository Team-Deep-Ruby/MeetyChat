namespace MeetyChat.Tests
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
    using Services.Models.Rooms;

    [TestClass]
    public class RoomsControllerTests
    {
        private RoomsController controller;
        private JavaScriptSerializer serializer;
        private MeetyChatDataMock unitOfWork;

        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWork = new MeetyChatDataMock();

            this.controller = new RoomsController(this.unitOfWork, UserIdProviderMock.GetUserIdProvider().Object);
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

        [TestMethod]
        public void GettingUsersByRoomShouldReturnUsers()
        {
            var roomUser =
                this.unitOfWork.Rooms.All()
                    .FirstOrDefault(r => r.Id == 1)
                    .Members.First();

            var httpResponse = this.controller.GetUsersByRoom(1).ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            var users = this.serializer.Deserialize<IList<UserOutputModel>>(serverResponseJson);

            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(users.First().Id, roomUser.Id);
        }

        [TestMethod]
        public void GettingUsersByRoomWithInvalidRoomIdShouldFail()
        {
            var httpResponse = this.controller.GetUsersByRoom(5).ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual("{\"Message\":\"Invalid room id\"}", serverResponseJson);
        }

        [TestMethod]
        public void GetRoomByIdShouldReturnRoom()
        {
            var expectedRoom = new RoomViewModel()
            {
                Name = "room 1"
            };

            var httpResponse = this.controller
                .GetRoomById(1)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            var room = this.serializer.Deserialize<RoomViewModel>(serverResponseJson);

            Assert.AreEqual(expectedRoom.Name, room.Name);
        }

        [TestMethod]
        public void GetRoomByIdWhenRoomDoesNotExistShouldFail()
        {
            var httpResponse = this.controller
                    .GetRoomById(5)
                    .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual("{\"Message\":\"Such room does not exist\"}", serverResponseJson);
        }

        [TestMethod]
        public void AddingRoomShouldAddRoom()
        {
            var room = new RoomBindingModel()
            {
                Name = "new room"
            };

            var httpResponse = this.controller.AddRoom(room)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var resultRoomName =
                this.unitOfWork.Rooms.All()
                .Select(r => r.Name)
                .Last();

            Assert.AreEqual(room.Name, resultRoomName);
        }

        [TestMethod]
        public void DeleteRoomShouldDeleteRoom()
        {
            var room = this.unitOfWork.Rooms.All().First();
            var initialRoomsCount =
                this.unitOfWork.Rooms.All().Count();

            var httpResponse = this.controller.DeleteRoom(1)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            Assert.AreNotEqual(room.Id, 
                this.unitOfWork.Rooms.All().First().Id);

            Assert.AreEqual(initialRoomsCount - 1,
                this.unitOfWork.Rooms.All().Count());
        }

        [TestMethod]
        public void DeleteRoomWhenRoomDoesNotExistShouldFail()
        {
            var httpResponse = this.controller.DeleteRoom(5)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [TestMethod]
        public void JoinRoomShouldJoinRoom()
        {
            var initialMembersCount =
                this.unitOfWork.Rooms.All()
                    .FirstOrDefault(r => r.Id == 1)
                    .Members.Count;

            var httpResponse = this.controller.JoinRoom(1)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var resultMembersCount =
                this.unitOfWork.Rooms.All()
                    .FirstOrDefault(r => r.Id == 1)
                    .Members.Count;

            Assert.AreEqual(initialMembersCount + 1, resultMembersCount);

            var joinedUser =
                this.unitOfWork.Rooms.All()
                    .FirstOrDefault(r => r.Id == 1)
                    .Members.Last();

            var expectedUser =
                this.unitOfWork.Users.All()
                    .First();

            Assert.AreEqual(expectedUser, joinedUser);

            var newLog = this.unitOfWork.RoomsJoiningHistory.All()
                .Last();

            Assert.AreEqual(expectedUser, newLog.User);
        }

        [TestMethod]
        public void JoinRoomWhenRoomDoesNotExistShouldFail()
        {
            var httpResponse = this.controller.JoinRoom(5)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [TestMethod]
        public void LeaveRoomShouldLeaveRoom()
        {
            var joinRoomHttpResponse = 
                this.controller.JoinRoom(1)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, joinRoomHttpResponse.StatusCode);

            var initialMembersCount =
                this.unitOfWork.Rooms.All()
                    .FirstOrDefault(r => r.Id == 1)
                    .Members.Count;

            var leaveRoomHttpResponse = 
                this.controller.LeaveRoom(1)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, leaveRoomHttpResponse.StatusCode);

            var resultMembersCount =
                this.unitOfWork.Rooms.All()
                    .FirstOrDefault(r => r.Id == 1)
                    .Members.Count;

            Assert.AreEqual(initialMembersCount - 1,
                resultMembersCount);

            var newLog = this.unitOfWork.RoomsJoiningHistory.All()
                .Last();

            Assert.IsNotNull(newLog.LeftOn);
        }

        [TestMethod]
        public void LeaveRoomWhenRoomDoesNotExistShouldFail()
        {
            var leaveRoomHttpResponse =
                this.controller.LeaveRoom(5)
                .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, leaveRoomHttpResponse.StatusCode);
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
