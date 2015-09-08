namespace MeetyChat.Tests.Mocks
{
    using Data.Data;
    using Data.Interfaces;
    using MeetyChat.Models;
    using Models;

    public class MeetyChatDataMock : IMeetyChatData
    {
        private IRepository<ApplicationUser> users;
        private IRepository<Room> rooms;
        private IRepository<Message> messages;

        public MeetyChatDataMock()
        {
            this.users = new RepositoryMock<ApplicationUser>();
            this.rooms = new RepositoryMock<Room>();
            this.messages = new RepositoryMock<Message>();
        }

        public bool IsSaveCalled { get; set; }

        public IRepository<ApplicationUser> Users
        {
            get { return this.users; }
        }

        public IRepository<Room> Rooms
        {
            get { return this.rooms; }
        }

        public IRepository<Message> Messages
        {
            get { return this.messages; }
        }

        public IRepository<UserSession> UserSessions
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public IRepository<RoomsJoiningHistory> RoomsJoiningHistory
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public int SaveChanges()
        {
            this.IsSaveCalled = true;
            return 1;
        }
    }
}