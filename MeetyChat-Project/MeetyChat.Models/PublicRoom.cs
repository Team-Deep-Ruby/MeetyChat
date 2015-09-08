namespace MeetyChat.Models
{
    using System.Collections.Generic;

    public class PublicRoom : Room
    {
        private ICollection<ApplicationUser> members;

        public PublicRoom()
        {
            this.members = new HashSet<ApplicationUser>();
        }

        public int MembersCount
        {
            get { return this.Members.Count; }
        }

        public virtual ICollection<ApplicationUser> Members
        {
            get { return this.members; }
            set { this.members = value; }
        }
    }
}
