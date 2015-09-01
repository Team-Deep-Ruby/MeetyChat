namespace MeetyChat.Data.Interfaces
{
    using Models;

    public interface IMeetyChatData
    {
        IRepository<ApplicationUser> Users { get; }
        IRepository<Room> Rooms { get; }
        IRepository<Message> Messages { get; }
        IRepository<UserSession> UserSessions { get; }
        IRepository<RoomsJoiningHistory> RoomsJoiningHistory { get; }

        int SaveChanges();
    }
}
