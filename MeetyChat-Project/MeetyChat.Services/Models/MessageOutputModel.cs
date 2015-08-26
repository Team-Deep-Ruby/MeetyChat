namespace MeetyChat.Services.Models
{
    using System;

    public class MessageOutputModel
    {
        public DateTime Date { get; set; }

        public string SenderId { get; set; }

        public int Id { get; set; }

        public string Content { get; set; }
    }
}