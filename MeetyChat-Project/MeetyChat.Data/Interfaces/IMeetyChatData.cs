namespace MeetyChat.Data.Interfaces
{
    using Models;

    public interface IMeetyChatData
    {
        IRepository<ApplicationUser> Users { get; }
        IRepository<PublicRoom> PublicRooms { get; }
        IRepository<PrivateRoom> PrivateRooms { get; }
        IRepository<Message> Messages { get; }
        IRepository<UserSession> UserSessions { get; }
        IRepository<RoomsJoiningHistory> RoomsJoiningHistory { get; }

        int SaveChanges();
    }
}
