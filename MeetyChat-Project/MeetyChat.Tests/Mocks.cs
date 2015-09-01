namespace MeetyChat.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Interfaces;
    using Models;
    using Moq;
    using Services.Infrastructure;

    public class Mocks
    {
        public const string MockUserId = "UserIdMock";

        public static Mock<IUserIdProvider> GetUserIdProvider()
        {
            var userIdProviderMock = new Mock<IUserIdProvider>();

            userIdProviderMock.Setup(x => x.GetUserId()).
                Returns(MockUserId);

            return userIdProviderMock;
        }

        public static Mock<IRepository<Message>> GetMessageRepositoryMock()
        {
            var repositoryMock = new Mock<IRepository<Message>>();

            var messageList = GetMockedMessagesList();

            repositoryMock.Setup(x => x.All()).Returns(() =>
                messageList
                .AsQueryable());

            return repositoryMock;
        }

        public static Mock<IRepository<Room>> GetRoomRepositoryMock()
        {
            var repositoryMock = new Mock<IRepository<Room>>();

            var roomsList = GetMockedRoomsList();

            repositoryMock.Setup(x => x.All()).Returns(() =>
                roomsList
                .AsQueryable());

            return repositoryMock;
        }

        public static Mock<IMeetyChatData> GetUnitOfWorkMock()
        {
            var unitOfWorkMock = new Mock<IMeetyChatData>();

            unitOfWorkMock.Setup(u => u.Messages).Returns(() => GetMessageRepositoryMock().Object);
            unitOfWorkMock.Setup(u => u.Rooms).Returns(() => GetRoomRepositoryMock().Object);

            return unitOfWorkMock;
        }

        private static IList<Message> GetMockedMessagesList()
        {
            return new List<Message>()
            {
                new Message()
                {
                    Content = "Message 1",
                    Date = new DateTime(2010, 5, 5),
                    Id = 1,
                    SenderId = MockUserId,
                    Sender = GetMockedUser(),
                    RoomId = 1
                },
                new Message()
                {
                    Content = "Message 2",
                    Date = new DateTime(2014, 4, 9),
                    Id = 2,
                    SenderId = MockUserId,
                    Sender = GetMockedUser(),
                    RoomId = 1
                },
                new Message()
                {
                    Content = "Message 3",
                    Date = new DateTime(2015, 3, 2),
                    Id = 3,
                    SenderId = MockUserId,
                    Sender = GetMockedUser(),
                    RoomId = 1
                }
            };
        }

        private static IEnumerable<Room> GetMockedRoomsList()
        {
            return new List<Room>()
            {
                new Room()
                {
                    Id = 1,
                    Name = "room 1",
                    Members = new List<ApplicationUser>(){GetMockedUser()},
                    Messages = GetMockedMessagesList()
                },
                new Room()
                {
                    Id = 2,
                    Name = "room 2",
                    Members = new List<ApplicationUser>(){GetMockedUser()},
                    Messages = GetMockedMessagesList()
                },
                new Room()
                {
                    Id = 3,
                    Name = "room 3",
                    Members = new List<ApplicationUser>(){GetMockedUser()},
                    Messages = GetMockedMessagesList()
                },
            };
        } 

        private static ApplicationUser GetMockedUser()
        {
            return new ApplicationUser()
            {   
                Email = "user@user.bg",
                Gender = Gender.Male,
                UserName = "user",
                Id = MockUserId
            };
        }
    }
}