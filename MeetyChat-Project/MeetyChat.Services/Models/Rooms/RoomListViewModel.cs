namespace MeetyChat.Services.Models.Rooms
{
    using System;
    using System.Linq.Expressions;
    using MeetyChat.Models;

    public class RoomListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MembersCount { get; set; }


        public static Expression<Func<Room, RoomListViewModel>> Create
        {
            get
            {
                return r => new RoomListViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    MembersCount = r.Members.Count,
                };
            }
        }
    }
}