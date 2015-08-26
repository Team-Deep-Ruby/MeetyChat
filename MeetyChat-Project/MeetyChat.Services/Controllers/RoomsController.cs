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
        private IUserIdProvider provider;

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
        public IHttpActionResult DeleteRoom(int id)
        {
            var room = this.data.Rooms.All().FirstOrDefault(r => r.Id == id);

            if (room == null)
            {
                return this.BadRequest("Such room does not exist");
            }

            this.data.Rooms.Delete(room);
            this.data.SaveChanges();

            return this.Ok("Room deleted successfully");
        }
    }
}
