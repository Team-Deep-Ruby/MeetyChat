namespace MeetyChat.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public abstract class Room
    {
        private ICollection<Message> messages;

        protected Room()
        {
            this.messages = new HashSet<Message>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        public string Name { get; set; }

        public virtual ICollection<Message> Messages
        {
            get { return this.messages; }
            set { this.messages = value; }
        }
    }
}