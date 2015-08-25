namespace MeetyChat.Models
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser
    {
        private ICollection<Room> rooms;
        private ICollection<Message> sendMessages; 

        public ApplicationUser()
        {
            this.rooms = new HashSet<Room>();
            this.sendMessages = new HashSet<Message>(); 
        }

        public string Name { get; set; }

        public string ProfileDataPicture { get; set; }

        public Gender Gender { get; set; }

        public virtual ICollection<Room> Rooms
        {
            get { return this.rooms; }
            set { this.rooms = value; }
        }

        public virtual ICollection<Message> SendMessages
        {
            get { return this.sendMessages; }
            set { this.sendMessages = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(
                this,
                authenticationType);

            return userIdentity;
        }
    }
}
