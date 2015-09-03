namespace MeetyChat.Services.Models.Messages
{
    using System;
    using System.Linq.Expressions;
    using MeetyChat.Models;

    public class MessageViewModel
    {
        public int Id { get; set; }

        public string SenderName { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public static Expression<Func<Message, MessageViewModel>> Create
        {
            get
            {
                return m => new MessageViewModel
                {
                    Id = m.Id,
                    SenderName = m.Sender.Name,
                    Content = m.Content,
                    Date = m.Date
                };
            }
        }
    }
}