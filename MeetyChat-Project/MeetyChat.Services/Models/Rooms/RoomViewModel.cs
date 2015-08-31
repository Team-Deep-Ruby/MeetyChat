namespace MeetyChat.Services.Models.Rooms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using MeetyChat.Models;
    using Messages;

    public class RoomViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MembersCount { get; set; }

        public IEnumerable<MessageViewModel> Messages { get; set; }

        public static Expression<Func<Room, RoomViewModel>> Create
        {
            get
            {
                return r => new RoomViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    MembersCount = r.Members.Count,
                    Messages = r.Messages
                        .Select(m => new MessageViewModel
                        {
                            Id = m.Id,
                            SenderId = m.SenderId,
                            Content = m.Content,
                            Date = m.Date
                        })
                };
            }
        }
    }
}