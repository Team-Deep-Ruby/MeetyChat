namespace MeetyChat.Services.Models.Messages
{
    using System;
    using System.Linq.Expressions;
    using MeetyChat.Models;

    public class MessageViewModel
    {
        public int Id { get; set; }

        public string SenderId { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public static Expression<Func<Message, MessageViewModel>> Create
        {
            get
            {
                return m => new MessageViewModel
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    Content = m.Content,
                    Date = m.Date
                };
            }
        }
    }
}