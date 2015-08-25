namespace MeetyChat.Models
{
    using System.Collections.Generic;

    public class Room
    {
        private ICollection<Message> messages;
        private ICollection<ApplicationUser> members; 

        public Room()
        {
            this.messages = new HashSet<Message>();
            this.members = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }

        public virtual ICollection<Message> Messages
        {
            get { return this.messages; }
            set { this.messages = value; }
        }

        public virtual ICollection<ApplicationUser> Members
        {
            get { return this.members; }
            set { this.members = value; }
        }
    }
}