namespace MeetyChat.Services.Models.Rooms
{
    using System;
    using System.Linq.Expressions;
    using MeetyChat.Models;

    public class PrivateRoomsListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static Expression<Func<PrivateRoom, PrivateRoomsListViewModel>> Create
        {
            get
            {
                return r => new PrivateRoomsListViewModel
                {
                    Id = r.Id,
                    Name = r.Name
                };
            }
        }
    }
}