namespace MeetyChat.Models
{
    using System;

    public class Message
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public virtual Room Room { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }
    }
}