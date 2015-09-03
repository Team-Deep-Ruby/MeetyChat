namespace MeetyChat.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;
    using Data.Data;
    using Data.Interfaces;
    using Mocks;
    using Models;
    using Moq;
    using Services.Infrastructure;

    public class PublicMocks
    {
        public const string MockUserId = "UserIdMock";

        public static Mock<IUserIdProvider> GetUserIdProvider()
        {
            var userIdProviderMock = new Mock<IUserIdProvider>();

            userIdProviderMock.Setup(x => x.GetUserId()).
                Returns(MockUserId);

            return userIdProviderMock;
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

        public static MeetyChatDataMock GetUnitOfWorkMock()
        {
            var data = new MeetyChatDataMock();
            PopulateMockMessages(data);
            PopulateMockRooms(data);
            PopulateMockUsers(data);

            return data;
        }

        public static IList<Message> GetMockedMessagesList()
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

        public static IList<Room> GetMockedRoomsList()
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

        public static ApplicationUser GetMockedUser()
        {
            return new ApplicationUser()
            {   
                Email = "user@user.bg",
                Gender = Gender.Male,
                UserName = "user",
                Id = MockUserId
            };
        }

        private static void PopulateMockUsers(MeetyChatDataMock data)
        {
            data.Users.Add(PublicMocks.GetMockedUser());
        }

        private static void PopulateMockRooms(MeetyChatDataMock data)
        {
            data.Rooms.Add(new Room()
            {
                Id = 1,
                Name = "room 1",
                Members = new List<ApplicationUser>() { PublicMocks.GetMockedUser() },
                Messages = data.Messages.All().ToList()
            });

            data.Rooms.Add(new Room()
            {
                Id = 2,
                Name = "room 2",
                Members = new List<ApplicationUser>() { GetMockedUser() },
                Messages = data.Messages.All().ToList()
            });

            data.Rooms.Add(new Room()
            {
                Id = 3,
                Name = "room 3",
                Members = new List<ApplicationUser>() { PublicMocks.GetMockedUser() },
                Messages = data.Messages.All().ToList()
            });
        }

        private static void PopulateMockMessages(MeetyChatDataMock data)
        {
            data.Messages.Add(new Message()
            {
                Content = "Message 1",
                Date = new DateTime(2010, 5, 5),
                Id = 1,
                SenderId = PublicMocks.MockUserId,
                Sender = PublicMocks.GetMockedUser(),
                RoomId = 1
            });

            data.Messages.Add(new Message()
            {
                Content = "Message 2",
                Date = new DateTime(2014, 4, 9),
                Id = 2,
                SenderId = PublicMocks.MockUserId,
                Sender = PublicMocks.GetMockedUser(),
                RoomId = 1
            });

            data.Messages.Add(new Message()
            {
                Content = "Message 3",
                Date = new DateTime(2015, 3, 2),
                Id = 3,
                SenderId = PublicMocks.MockUserId,
                Sender = PublicMocks.GetMockedUser(),
                RoomId = 1
            });
        }
    }
}