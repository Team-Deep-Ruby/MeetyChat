﻿namespace MeetyChat.Data.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Interfaces;
    using Migrations;
    using Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class MeetyChatDbContext : IdentityDbContext<ApplicationUser>, IMeetyChatDbContext
    {
        public MeetyChatDbContext()
         : base("MeetyChat")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MeetyChatDbContext, Configuration>());
        }

        public IDbSet<Message> Messages { get; set; }
        public IDbSet<Room> Rooms { get; set; }
        public IDbSet<UserSession> UserSessions { get; set; }
        public IDbSet<RoomsJoiningHistory> RoomsJoiningHistories { get; set; }

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