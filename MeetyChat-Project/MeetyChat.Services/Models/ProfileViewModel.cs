namespace MeetyChat.Services.Models
{
    using MeetyChat.Models;

    public class ProfileViewModel
    {
        public object Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string ProfileImage { get; set; }

        public Gender Gender { get; set; }

        /*id = user.Id,
                username = user.UserName,
                name = user.Name,
                email = user.Email,
                profileImage = user.ProfileImage,
                gender = user.Gender
         */
    }
}