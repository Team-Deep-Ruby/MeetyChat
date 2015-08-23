namespace MeetyChat.Data
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MeetyChat.Models;
    using Migrations;

    public class MeetyChatDbContext : IdentityDbContext<ApplicationUser>
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

        public static MeetyChatDbContext Create()
        {
            return new MeetyChatDbContext();
        }
    }
}