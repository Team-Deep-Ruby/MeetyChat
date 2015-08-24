namespace MeetyChat.Data.Interfaces
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Models;

    public interface IMeetyChatDbContext
    {
        IDbSet<Message> Messages { get; set; }
        IDbSet<Room> Rooms { get; set; }
        IDbSet<ApplicationUser> Users { get; set; }

        int SaveChanges();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        IDbSet<T> Set<T>() where T : class;
    }
}