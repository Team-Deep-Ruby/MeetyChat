namespace MeetyChat.Services.Models
{
    using System;
    using System.Linq.Expressions;
    using MeetyChat.Models;


    public class MessageOutputModel
    {
        public DateTime Date { get; set; }

        public string SenderId { get; set; }

        public string SenderName { get; set; }

        public int Id { get; set; }

        public string Content { get; set; }

        public int RoomId { get; set; }
    }
}