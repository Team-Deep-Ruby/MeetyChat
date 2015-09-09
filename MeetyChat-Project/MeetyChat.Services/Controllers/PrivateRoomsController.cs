namespace MeetyChat.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Data.Interfaces;
    using Infrastructure;
    using MeetyChat.Models;
    using Models;
    using Models.Rooms;
    using Ninject.Web.WebApi;
    using UserSessionUtils;

    [SessionAuthorize]
    public class PrivateRoomController : BaseApiController
    {
        private readonly IUserIdProvider provider;

        public PrivateRoomController(IMeetyChatData data, IUserIdProvider provider)
            : base(data)
        {
            this.provider = provider;
        }

        [HttpGet]
        [Route("api/privateRooms")]
        public IHttpActionResult GetPrivateRooms()
        {
            var currentUserId = this.data.Users.All()
                .First(u => u.Name == this.User.Identity.Name)
                .Id;

            var rooms = this.data.PrivateRooms.All()
                .Where(r => r.FirstUserId == currentUserId
                    || r.SecondUserId == currentUserId)
                .Select(PrivateRoomsListViewModel.Create);

            return this.Ok(rooms);
        }

        [HttpGet]
        [Route("api/privateRooms/{id}")]
        public IHttpActionResult GetPrivateRoomById(int id)
        {
            var room = this.data.PrivateRooms.All()
                .Select(PrivateRoomsListViewModel.Create)
                .FirstOrDefault(r => r.Id == id);

            if (room == null)
            {
                return this.BadRequest("Such room does not exist");
            }

            var currentUserName = this.User.Identity.Name;
            var roomNames = room.Name.Split(' ');
            if (roomNames[0].Equals(currentUserName) || roomNames[1].Equals(currentUserName))
            {
                return this.Ok(room);
            }
            
            return this.BadRequest("You can't join this room.");
        }

        [HttpPost]
        [Route("api/privateRooms")]
        public IHttpActionResult AddPrivateRoom(PrivateRoomBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (model.FirstUsername == model.SecondUsername)
            {
                return this.BadRequest("You cannot chat with yourself.");
            }

            var privateRoomFromDb = this.data.PrivateRooms.All().FirstOrDefault(
                r => r.Name == model.FirstUsername + " " + model.SecondUsername
                || r.Name == model.SecondUsername + " " + model.FirstUsername);

            if (privateRoomFromDb != null)
            {
                return this.JoinPrivateRoom(privateRoomFromDb.Id);
            }

            var firstUser = this.data.Users.All().First(u => u.Name == model.FirstUsername);
            var secondUser = this.data.Users.All().First(u => u.Name == model.SecondUsername);

            var room = new PrivateRoom
            {
                Name = firstUser.Name + " " + secondUser.Name,
                FirstUserId = firstUser.Id,
                SecondUserId = secondUser.Id
            };

            this.data.PrivateRooms.Add(room);
            this.data.SaveChanges();

            return this.JoinPrivateRoom(room.Id);
        }

        [HttpPut]
        [Route("api/rooms/{id}/joinPrivateRoom")]
        public IHttpActionResult JoinPrivateRoom(int id)
        {
            var userId = this.provider.GetUserId();

            var user = this.data.Users.GetById(userId);
            var room = this.data.PrivateRooms.GetById(id);

            RoomsJoiningHistory log = new RoomsJoiningHistory()
            {
                JoinedOn = DateTime.Now,
                Room = room,
                User = user
            };

            if (room == null)
            {
                return this.BadRequest("Such room does not exist");
            }

            this.data.RoomsJoiningHistory.Add(log);
            this.data.SaveChanges();
            return this.Ok(room.Id);
        }
    }
}
