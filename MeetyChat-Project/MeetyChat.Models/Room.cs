namespace MeetyChat.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Room
    {
        private ICollection<Message> messages;
        private ICollection<ApplicationUser> members; 

        public Room()
        {
            this.messages = new HashSet<Message>();
            this.members = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        public string Name { get; set; }

        public int MembersCount
        {
            get { return this.Members.Count; }
        }

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