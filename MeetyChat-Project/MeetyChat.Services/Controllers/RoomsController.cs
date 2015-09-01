﻿namespace MeetyChat.Services.Controllers
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
                .Select(r => new RoomViewModel
                {
                
                    Id = r.Id,
                    Name = r.Name,
                    MembersCount = r.Members.Count
                });
            
            return this.Ok(rooms);
        }

        [HttpGet]
        [Route("api/rooms/{roomId}/users")]
        public IHttpActionResult GetUsersByRoom(int roomId)
        {
            var room = this.data.Rooms.All().FirstOrDefault(r => r.Id == roomId);

            if (room == null)
            {
                return this.BadRequest("Invalid room id");
            }

            var members = room.Members.Select(m => new
            {
                Id = m.Id,
                Username = m.UserName
            }).AsQueryable();

            return this.Ok(members);
        }

        [HttpGet]
        [Route("api/rooms/{roomId}/users/latest/left")]
        public IHttpActionResult GetUsersWhoLeftByRoom(int roomId)
        {
            DateTime date = DateTime.Now;
            TimeSpan timeout = new TimeSpan(0, 0, 2, 0); // 2 minutes timeout

            var room = this.data.Rooms.All().FirstOrDefault(r => r.Id == roomId);

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
                        Id = rjh.User.Id,
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
        [Route("api/rooms/{roomId}/users/latest/joined")]
        public IHttpActionResult GetLatestUsersByRoom(int roomId)
        {

            DateTime date = DateTime.Now;
            TimeSpan timeout = new TimeSpan(0, 0, 2, 0); // 2 minutes timeout

            var room = this.data.Rooms.All().FirstOrDefault(r => r.Id == roomId);

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
                        Id = l.User.Id,
                        Username = l.User.UserName,
                        JoinedOn = l.JoinedOn,
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
                    }),
                    Messages = r.Messages
                        .Select(m => new 
                        {
                            Id = m.Id,
                            SenderId = m.SenderId,
                            Content = m.Content,
                            Date = m.Date
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
            var roomJoinLog = this.data.RoomsJoiningHistory.All()
                .FirstOrDefault(rjh => rjh.Room.Id == id && rjh.User.Id == userId);

            roomJoinLog.LeftOn = DateTime.Now;
            this.data.SaveChanges();
            return this.Ok("Room left successfully");
        }
    }
}
