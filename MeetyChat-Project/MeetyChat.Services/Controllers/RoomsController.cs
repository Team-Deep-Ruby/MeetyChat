namespace MeetyChat.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Data.Interfaces;
    using Infrastructure;
    using MeetyChat.Models;
    using Models;

    public class RoomsController : BaseApiController
    {
        private readonly IUserIdProvider provider;

        public RoomsController(IMeetyChatData data, IUserIdProvider provider) 
            : base(data)
        {
            this.provider = provider;
        }

        [HttpGet]
        public IHttpActionResult GetAllRooms()
        {
            var rooms = this.data.Rooms.All()
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    Members = r.Members.Count
                });

            return this.Ok(rooms);
        }

        [HttpGet]
        [Route("api/rooms/{roomId}/users")]
        public IHttpActionResult GetUsersByRoom(int roomId)
        {
            return this.Ok();
        }

        [HttpGet]
        public IHttpActionResult GetRoomById(int id)
        {
            var room = this.data.Rooms.All()
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    MembersCount = r.Members.Count,
                    Members = r.Members.Select(m => new
                    {
                        m.Name,
                        m.Gender
                    })
                })
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

            var room = new Room() {Name = model.Name};

            this.data.Rooms.Add(room);
            this.data.SaveChanges();

            return this.Ok(room);
        }

        [HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteRoom(int id)
        {
            var room = this.data.Rooms.GetById(id);

            if (room == null)
            {
                return this.BadRequest("Such room does not exist");
            }

            this.data.Rooms.Delete(room);
            this.data.SaveChanges();

            return this.Ok("Room deleted successfully");
        }

        [HttpPut]
        [Authorize]
        [Route("api/rooms/{id}/join")]
        public IHttpActionResult JoinRoom(int id)
        {
            var userId = this.provider.GetUserId();

            var user = this.data.Users.GetById(userId);
            var room = this.data.Rooms.GetById(id);

            if (room == null)
            {
                return this.BadRequest("Such room does not exist");
            }

            room.Members.Add(user);
            this.data.SaveChanges();
            return this.Ok("Room joined successfully");
        }

        [HttpPut]
        [Authorize]
        [Route("api/rooms/{id}/leave")]
        public IHttpActionResult LeaveRoom(int id)
        {
            var userId = this.provider.GetUserId();

            var user = this.data.Users.GetById(userId);
            var room = this.data.Rooms.GetById(id);

            if (room == null)
            {
                return this.BadRequest("Such room does not exist");
            }

            room.Members.Remove(user);
            this.data.SaveChanges();
            return this.Ok("Room left successfully");
        }
    }
}
