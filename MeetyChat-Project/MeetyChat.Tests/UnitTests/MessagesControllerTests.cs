namespace MeetyChat.Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Services.Controllers;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Hosting;
    using System.Web.Http.Routing;
    using System.Web.Script.Serialization;
    using MeetyChat.Models;
    using Mocks;
    using Services.Models;
    using Services.Models.InputModels;

    [TestClass]
    public class MessagesControllerTests
    {
        private MessagesController controller;
        private JavaScriptSerializer serializer;
        private MeetyChatDataMock unitOfWorkMock;

        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWorkMock = new MeetyChatDataMock();

            this.controller = new MessagesController(this.unitOfWorkMock, UserIdProviderMock.GetUserIdProvider().Object);
            this.serializer = new JavaScriptSerializer();
            this.SetupController();
        }

        [TestMethod]
        public void TestGettingAllMessagesShouldReturnAllMessages()
        {
            var httpResponse = this.controller.GetAllMessages(1).ExecuteAsync(new CancellationToken()).Result;

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var messages = this.serializer.Deserialize<IList<MessageOutputModel>>(serverResponseJson)
                .Select(m => m.Id)
                .ToList();

            var expectedResult = GetExpectedMessagesResult()
                .Select(m => m.Id)
                .ToList();

            CollectionAssert.AreEqual(expectedResult, messages);
        }

        [TestMethod]
        public void GettingAllMessagesWithInvalidRoomIdShouldFail()
        {
            var httpResponse = this.controller.GetAllMessages(5).ExecuteAsync(new CancellationToken()).Result;

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);

            Assert.AreEqual("{\"Message\":\"Invalid room id\"}", serverResponseJson);
        }

        [TestMethod]
        public void GettingAllMessagesForEmptyRoomShouldReturnZeroMessages()
        {
            this.unitOfWorkMock.PublicRooms.Add(new PublicRoom
            {
                Id = 5,
                Name = "Room 5",
                Messages = new Message[0]
            });

            var httpResponse = this.controller.GetAllMessages(5).ExecuteAsync(new CancellationToken()).Result;

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var messagesCount = this.serializer.Deserialize<IList<MessageOutputModel>>(serverResponseJson)
                .Count;

            Assert.AreEqual(0, messagesCount);
        }

        [TestMethod]
        public void AddingMessageShouldAddMessage()
        {
            var message = new MessageInputModel()
            {
                Content = "New message"
            };

            var expectedMessage = new MessageOutputModel()
            {
                Content = "New message",
                Date = DateTime.Now,
                Id = 4,
                RoomId = 1,
                SenderId = MeetyChatDataMock.MockUserId
            };

            var httpResponse = this.controller.AddMessage(1, message).ExecuteAsync(new CancellationToken()).Result;

            var serverResponse = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(serverResponse, "\"Message created successfully\"");

            Assert.AreEqual(expectedMessage.Content, this.unitOfWorkMock.Messages.All().Last().Content);
        }

        [TestMethod]
        public void AddingMessageWithInvalidRoomIdShouldFail()
        {
            var message = new MessageInputModel()
            {
                Content = "New message"
            };

            var httpResponse = this.controller.AddMessage(5, message).ExecuteAsync(new CancellationToken()).Result;

            var serverResponse = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual("{\"Message\":\"Invalid room id\"}", serverResponse);

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [TestMethod]
        public void GettingLatestMessagesWithInvalidRoomIdShouldFail()
        {
            var httpResponse = this.controller
                .GetLatestMessages(5)
                .ExecuteAsync(new CancellationToken()).Result;

            var serverResponse = httpResponse.Content
                .ReadAsStringAsync().Result;

            Assert.AreEqual("{\"Message\":\"Invalid room id\"}", serverResponse);

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        /*
        [TestMethod]
        public void GettingLatestMessagesShouldReturnLatestMessages()
        {
            var message = new MessageInputModel()
            {
                Content = "New message"
            };

            var addMessageResponse = this.controller.AddMessage(1, message).ExecuteAsync(new CancellationToken());

            var httpResponseTask = this.controller
                    .GetLatestMessages(1)
                    .ExecuteAsync(new CancellationToken()).Result;

            var httpResponse = httpResponseTask;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var serverResponse = httpResponse.Content
                .ReadAsStringAsync().Result;

            var messageId = this.serializer.Deserialize<IList<MessageOutputModel>>(serverResponse)
                .Select(m => m.Id)
                .FirstOrDefault();

            Assert.AreEqual(messageId, this.unitOfWorkMock.Messages.All().Last().Id);
        }
        */

        /*
        [TestMethod]
        public void AddingMessageWithInvalidInputModelShouldFail()
        {
            var message = new MessageInputModel();

            var httpResponse = this.controller.AddMessage(1, message).ExecuteAsync(new CancellationToken()).Result;

            var serverResponse = httpResponse.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(serverResponse, "\"Invalid input model\"");

            Assert.AreEqual(httpResponse.StatusCode, HttpStatusCode.BadRequest);
        }
        */

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

        private static IList<MessageOutputModel> GetExpectedMessagesResult()
        {
            return new List<MessageOutputModel>()
            {
                new MessageOutputModel()
                {
                    Id = 3,
                    Content = "Message 3",
                    Date = new DateTime(2015, 3, 2),
                    SenderId = MeetyChatDataMock.MockUserId,
                    RoomId = 1
                },
                new MessageOutputModel()
                {
                    Id = 2,
                    Content = "Message 2",
                    Date = new DateTime(2014, 4, 9),
                    SenderId = MeetyChatDataMock.MockUserId,
                    RoomId = 1
                },
                new MessageOutputModel()
                {
                    Id = 1,
                    Content = "Message 1",
                    Date = new DateTime(2010, 5, 5),
                    SenderId = MeetyChatDataMock.MockUserId,
                    RoomId = 1
                }
            };
        }
    }
}