namespace MeetyChat.Services.Models.Users
{
    using System;
    using System.Linq.Expressions;
    using MeetyChat.Models;

    public class MembersViewModel
    {
        public string Name { get; set; }

        public Gender Gender { get; set; }

        public string ProfileImage { get; set; }

        public static Expression<Func<ApplicationUser, MembersViewModel>> Create
        {
            get
            {
                return r => new MembersViewModel
                {
                    Name = r.Name,
                    Gender = r.Gender,
                    ProfileImage = r.ProfileImage
                };
            }
        }
    }
}