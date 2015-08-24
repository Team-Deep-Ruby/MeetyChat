namespace MeetyChat.Data.Interfaces
{
    using Models;

    public interface IMeetyChatData
    {
        IRepository<ApplicationUser> Users { get; }
        IRepository<Room> Rooms { get; }
        IRepository<Message> Messages { get; }

        int SaveChanges();
    }
}
