namespace MeetyChat.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Services.Controllers;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Hosting;
    using System.Web.Http.Results;
    using System.Web.Http.Routing;
    using System.Web.Script.Serialization;
    using Services.Models;

    [TestClass]
    public class MessagesControllerTests
    {
        private MessagesController controller;
        private JavaScriptSerializer serializer;

        [TestInitialize]
        public void Initialize()
        {
            this.controller = new MessagesController(Mocks.GetUnitOfWorkMock().Object, Mocks.GetUserIdProvider().Object);
            this.serializer = new JavaScriptSerializer();
            this.SetupController();
        }

        [TestMethod]
        public void TestGettingAllMessagesShouldReturnAllMessages()
        {

            var httpResponse = this.controller.GetAllMessages(1).ExecuteAsync(new CancellationToken()).Result;

            var serverResponseJson = httpResponse.Content.ReadAsStringAsync().Result;
            var messages = this.serializer.Deserialize<IList<MessageOutputModel>>(serverResponseJson);
            var newMessagesJson = serializer.Serialize(messages);

            var expectedResult = GetExpectedMessagesResult();
            var json = serializer.Serialize(expectedResult);

            Assert.AreEqual(json, newMessagesJson);
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
            
        private static IList<MessageOutputModel> GetExpectedMessagesResult()
        {
            return new List<MessageOutputModel>()
            {
                new MessageOutputModel()
                {
                    Id = 3,
                    Content = "Message 3",
                    Date = new DateTime(2015, 3, 2),
                    SenderId = Mocks.MockUserId,
                    RoomId = 1
                },
                new MessageOutputModel()
                {
                    Id = 2,
                    Content = "Message 2",
                    Date = new DateTime(2014, 4, 9),
                    SenderId = Mocks.MockUserId,
                    RoomId = 1
                },
                new MessageOutputModel()
                {
                    Id = 1,
                    Content = "Message 1",
                    Date = new DateTime(2010, 5, 5),
                    SenderId = Mocks.MockUserId,
                    RoomId = 1
                }
            };
        }
    }
}