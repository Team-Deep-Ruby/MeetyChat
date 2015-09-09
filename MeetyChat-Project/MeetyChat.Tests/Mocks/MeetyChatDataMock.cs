namespace MeetyChat.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Data;
    using Data.Interfaces;
    using MeetyChat.Models;
    using Models;

    public class MeetyChatDataMock : IMeetyChatData
    {
        public const string MockUserId = "UserIdMock";

        private IRepository<ApplicationUser> users;
        private IRepository<PublicRoom> rooms;
        private IRepository<PrivateRoom> privateRooms;
        private IRepository<Message> messages;
        private IRepository<RoomsJoiningHistory> history;
        private IRepository<UserSession> sessions; 

        public MeetyChatDataMock()
        {
            this.users = new RepositoryMock<ApplicationUser>();
            this.rooms = new RepositoryMock<PublicRoom>();
            this.privateRooms = new Repository<PrivateRoom>();
            this.messages = new RepositoryMock<Message>();
            this.history = new RepositoryMock<RoomsJoiningHistory>();
            this.sessions = new RepositoryMock<UserSession>();

            PopulateMockUsers(this);
            PopulateMockMessages(this);
            PopulateMockRooms(this);
        }

        public bool IsSaveCalled { get; set; }

        public IRepository<ApplicationUser> Users
        {
            get { return this.users; }
        }

        public IRepository<PublicRoom> PublicRooms
        {
            get { return this.rooms; }
        }

        public IRepository<PrivateRoom> PrivateRooms
        {
            get { return this.privateRooms; }
        }

        public IRepository<Message> Messages
        {
            get { return this.messages; }
        }

        public IRepository<UserSession> UserSessions
        {
            get { return this.sessions; }
        }

        public IRepository<RoomsJoiningHistory> RoomsJoiningHistory
        {
            get { return this.history; }
        }

        public int SaveChanges()
        {
            this.IsSaveCalled = true;
            return 1;
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

        public static IList<PublicRoom> GetMockedRoomsList()
        {
            return new List<PublicRoom>()
            {
                new PublicRoom()
                {
                    Id = 1,
                    Name = "room 1",
                    Members = new List<ApplicationUser>(){GetMockedUser()},
                    Messages = GetMockedMessagesList()
                },
                new PublicRoom()
                {
                    Id = 2,
                    Name = "room 2",
                    Members = new List<ApplicationUser>(){GetMockedUser()},
                    Messages = GetMockedMessagesList()
                },
                new PublicRoom()
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
            data.Users.Add(GetMockedUser());
        }

        private static void PopulateMockRooms(MeetyChatDataMock data)
        {
            data.PublicRooms.Add(new PublicRoom()
            {
                Id = 1,
                Name = "room 1",
                Members = new List<ApplicationUser>() { GetMockedUser() },
                Messages = data.Messages.All().ToList()
            });

            data.PublicRooms.Add(new PublicRoom()
            {
                Id = 2,
                Name = "room 2",
                Members = new List<ApplicationUser>() { GetMockedUser() },
                Messages = data.Messages.All().ToList()
            });

            data.PublicRooms.Add(new PublicRoom()
            {
                Id = 3,
                Name = "room 3",
                Members = new List<ApplicationUser>() { GetMockedUser() },
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
                SenderId = MockUserId,
                Sender = GetMockedUser(),
                RoomId = 1
            });

            data.Messages.Add(new Message()
            {
                Content = "Message 2",
                Date = new DateTime(2014, 4, 9),
                Id = 2,
                SenderId = MockUserId,
                Sender = GetMockedUser(),
                RoomId = 1
            });

            data.Messages.Add(new Message()
            {
                Content = "Message 3",
                Date = new DateTime(2015, 3, 2),
                Id = 3,
                SenderId = MockUserId,
                Sender = GetMockedUser(),
                RoomId = 1
            });
        }
    }
}