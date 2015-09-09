namespace MeetyChat.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Data;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MeetyChatDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MeetyChatDbContext context)
        {
            if (!context.Users.Any())
            {
                this.SeedApplicationUsers(context);
            }
        }

        private void SeedApplicationUsers(MeetyChatDbContext context)
        {
            var user = new ApplicationUser
                {
                    UserName = "qwerty",
                    Name = "qwerty",
                    Email = "qwerty@qwerty",
                    PasswordHash = "ABOYgM+IChRPgbaPEHn+7+4xWX5fRptDfktaFOkAFNEXrwkcryiU19UwqjJiZj+IBw==",
                    SecurityStamp = "fea5e506-178b-4674-84fb-6913845271c6"
                };

            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
