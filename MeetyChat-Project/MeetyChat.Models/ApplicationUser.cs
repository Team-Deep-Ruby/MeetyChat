namespace MeetyChat.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser
    {
        private ICollection<Room> rooms;

        public ApplicationUser()
        {
            this.rooms = new HashSet<Room>();
        }

        public string Name { get; set; }

        public string ProfileDataPicture { get; set; }

        public Gender Gender { get; set; }

        public virtual ICollection<Room> Rooms
        {
            get { return this.rooms; }
            set { this.rooms = value; }
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
