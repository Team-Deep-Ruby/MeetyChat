namespace MeetyChat.Services.Models.Rooms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using MeetyChat.Models;
    using Messages;
    using Users;

    public class RoomViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MembersCount { get; set; }

        public IEnumerable<MembersViewModel> Members { get; set; } 
        
        public static Expression<Func<PublicRoom, RoomViewModel>> Create
        {
            get
            {
                return r => new RoomViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    MembersCount = r.Members.Count,
                    Members = r.Members
                        .Select(m => new MembersViewModel
                    {
                        Name = m.Name,
                        Gender = m.Gender,
                    })
                };
            }
        }
    }
}