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
    using UserSessionUtils;

    [SessionAuthorize]
    public class PublicRoomsController : BaseApiController
    {
        private readonly IUserIdProvider provider;

        public PublicRoomsController(IMeetyChatData data, IUserIdProvider provider) 
            : base(data)
        {
            this.provider = provider;
        }

        [HttpGet]
        public IHttpActionResult GetAllRooms()
        {
            var rooms = this.data.PublicRooms.All()
                .Select(RoomListViewModel.Create);
            
            return this.Ok(rooms);
        }

        [HttpGet]
        [Route("api/publicRooms/{roomId}/users")]
        public IHttpActionResult GetUsersByRoom(int roomId)
        {
            var room = this.data.PublicRooms.All().FirstOrDefault(r => r.Id == roomId);

            if (room == null)
            {
                return this.BadRequest("Invalid room id");
            }

            var members = room.Members.Select(m => new UserOutputModel()
            {
                Id = m.Id,
                Username = m.UserName
            }).AsQueryable();

            return this.Ok(members);
        }

        [HttpGet]
        [Route("api/publicRooms/{roomId}/users/latest/left")]
        public IHttpActionResult GetUsersWhoLeftByRoom(int roomId)
        {
            DateTime date = DateTime.Now;
            TimeSpan timeout = new TimeSpan(0, 0, 2, 0); // 2 minutes timeout

            var room = this.data.PublicRooms.All().FirstOrDefault(r => r.Id == roomId);

            if (room == null)
            {
                return this.BadRequest("Invalid room id");
            }

            
            while (true)
            {
                var leftUsers = this.data.RoomsJoiningHistory.All()
                    .Where(rjh => rjh.LeftOn > date)
                    .Select(rjh => new
                    {
                        rjh.User.Id,
                        Username = rjh.User.UserName
                    });

                if (leftUsers.Any())
                {
                    return this.Ok(leftUsers);
                }
                else if (date - DateTime.Now > timeout)
                {
                    return this.Ok("No members have left the room");
                }
            }
        }

        [HttpGet]
        [Route("api/publicRooms/{roomId}/users/latest/joined")]
        public IHttpActionResult GetLatestUsersByRoom(int roomId)
        {

            DateTime date = DateTime.Now;
            TimeSpan timeout = new TimeSpan(0, 0, 2, 0); // 2 minutes timeout

            var room = this.data.PublicRooms.All().FirstOrDefault(r => r.Id == roomId);

            if (room == null)
            {
                return this.BadRequest("Invalid room id");
            }

            while (true)
            {
                var users = this.data.RoomsJoiningHistory.All()
                    .Where(rjh => rjh.JoinedOn > date && rjh.Room.Id == roomId)
                    .AsQueryable()
                    .Select(l => new
                    {
                        l.User.Id,
                        Username = l.User.UserName,
                        l.JoinedOn,
                        RoomId = l.Room.Id
                    });

                if (users.Any())
                {
                    return this.Ok(users);
                }
                else if (date - DateTime.Now > timeout)
                {
                    return this.Ok("No new room members were found");
                }
            }
        }

        [HttpGet]
        public IHttpActionResult GetRoomById(int id)
        {
            var room = this.data.PublicRooms.All()
                .Select(RoomViewModel.Create)
                .FirstOrDefault(r => r.Id == id);

            if (room == null)
            {
                return this.BadRequest("Such room does not exist");
            }

            return this.Ok(room);
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult AddRoom(RoomBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var checkForExistingRoom = this.data.PublicRooms.All()
                .FirstOrDefault(r => r.Name == model.Name);

            if (checkForExistingRoom != null)
            {
                return this.BadRequest("The chat room name already exists.");
            }

            var room = new PublicRoom {Name = model.Name};

            this.data.PublicRooms.Add(room);
            this.data.SaveChanges();

            return this.Ok(room);
        }

        [HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteRoom(int id)
        {
            var room = this.data.PublicRooms.All()
                .FirstOrDefault(r => r.Id == id);

            if (room == null)
            {
                return this.BadRequest("Such room does not exist");
            }

            this.data.PublicRooms.Delete(room);
            this.data.SaveChanges();

            return this.Ok("Room deleted successfully");
        }

        [HttpPut]
        [Authorize]
        [Route("api/publicRooms/{id}/join")]
        public IHttpActionResult JoinRoom(int id)
        {
            var userId = this.provider.GetUserId();

            var user = this.data.Users.All()
                .FirstOrDefault(u => u.Id == userId);
            var room = this.data.PublicRooms.All()
                .FirstOrDefault(r => r.Id == id);

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

            room.Members.Add(user);
            this.data.RoomsJoiningHistory.Add(log);
            this.data.SaveChanges();
            return this.Ok("Room joined successfully");
        }

        [HttpPut]
        [Authorize]
        [Route("api/publicRooms/{id}/leave")]
        public IHttpActionResult LeaveRoom(int id)
        {
            var userId = this.provider.GetUserId();

            var user = this.data.Users.All()
                .FirstOrDefault(u => u.Id == userId);
            var room = this.data.PublicRooms.All()
                .FirstOrDefault(r => r.Id == id);
                
            if (room == null)
            {
                return this.BadRequest("Such room does not exist");
            }

            room.Members.Remove(user);
            var roomJoinLog = this.data.RoomsJoiningHistory.All()
                .FirstOrDefault(rjh => rjh.Room.Id == id && rjh.User.Id == userId);

            roomJoinLog.LeftOn = DateTime.Now;
            this.data.SaveChanges();
            return this.Ok("Room left successfully");
        }
    }
}
