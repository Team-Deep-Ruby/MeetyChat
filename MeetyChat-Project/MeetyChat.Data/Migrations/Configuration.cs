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
            // Seed only if database is empty
            //if (!context.Users.Any())
            //{
            //    var users = this.SeedApplicationUsers(context);
            //}
        }

        //private IList<ApplicationUser> SeedApplicationUsers(MeetyChatDbContext context)
        //{
        //    var users = new List<ApplicationUser>()
        //    {
        //        new ApplicationUser()
        //        {
        //            UserName = "Batman",
        //            Name = "Bruce Wayne",
        //            Email = "man@bat.bg"
        //        }
        //    };

        //    var userStore = new UserStore<ApplicationUser>(context);
        //    var userManager = new UserManager<ApplicationUser>(userStore)
        //    {
        //        PasswordValidator = new PasswordValidator
        //        {
        //            RequiredLength = 2,
        //            RequireNonLetterOrDigit = false,
        //            RequireDigit = false,
        //            RequireLowercase = false,
        //            RequireUppercase = false
        //        }
        //    };

        //    foreach (var user in users)
        //    {
        //        var password = user.UserName;

        //        var userCreateResult = userManager.Create(user, password);
        //        if (!userCreateResult.Succeeded)
        //        {
        //            throw new Exception(string.Join(Environment.NewLine, userCreateResult.Errors));
        //        }
        //    }

        //    context.SaveChanges();

        //    return users;
        //}
    }
}
