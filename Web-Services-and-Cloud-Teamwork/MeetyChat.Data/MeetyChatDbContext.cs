namespace MeetyChat.Data
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Migrations;
    using Interfaces;

    public class MeetyChatDbContext : IdentityDbContext<ApplicationUser>, IMeetyChatDbContext
    {
        public MeetyChatDbContext()
         : base("MeetyChat")
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion
                    <MeetyChatDbContext, Configuration>());
        }

        public virtual IDbSet<Message> Messages { get; set; }
        public virtual IDbSet<Room> Rooms { get; set; }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public static MeetyChatDbContext Create()
        {
            return new MeetyChatDbContext();
        }
    }
}