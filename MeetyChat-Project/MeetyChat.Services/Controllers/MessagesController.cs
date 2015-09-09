namespace MeetyChat.Services.Controllers
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Web.Http;
	using System.Web.OData;
	using Data.Interfaces;
	using Infrastructure;
	using MeetyChat.Models;
	using Models;
	using Models.InputModels;
	using Models.Rooms;

	public class MessagesController : BaseApiController
	{
		private readonly IUserIdProvider userIdProvider;

		public MessagesController(IMeetyChatData data, IUserIdProvider userIdProvider)
			: base(data)
		{
			this.userIdProvider = userIdProvider;
		}

		[HttpGet]
		[EnableQuery]
		[Route("api/rooms/{roomId}/messages")]
		public IHttpActionResult GetAllMessages(int roomId)
		{
			var room = GetRoomById(roomId) ?? GetPrivateRoomById(roomId);  

			if (room == null)
			{
				return this.BadRequest("Invalid room id");
			}

			var messages = room.Messages
				.Select(m => new MessageOutputModel
				{
					Id = m.Id,
					Content = m.Content,
					Date = m.Date,
					SenderName = m.Sender.Name,
					RoomId = roomId
				})
				.OrderByDescending(m => m.Date);
			
			return this.Ok(messages);
		}

		[Authorize]
		[HttpPost]
		[Route("api/rooms/{roomId}/messages")]
		public IHttpActionResult AddMessage(int roomId, 
			MessageInputModel messageModel)
		{
			if (!this.ModelState.IsValid)
			{
				return this.BadRequest("Invalid input model");
			}

			var room = GetRoomById(roomId) ?? GetPrivateRoomById(roomId);

			if (room == null)
			{
				return this.BadRequest("Invalid room id");
			}

			var userId = this.userIdProvider.GetUserId();

			var sender = this.data.Users.All()
				.FirstOrDefault(u => u.Id == userId);

			if (sender == null)
			{
				return this.BadRequest("Invalid sender Id");
			}

			Message message = new Message()
			{
				RoomId = roomId,
				Room = room,
				Content = messageModel.Content,
				Date = DateTime.Now,
				SenderId = userId
			};

			this.data.Messages.Add(message);
			this.data.SaveChanges();

			return this.Ok("Message created successfully");
		}

		[HttpGet]
		[Route("api/rooms/{roomId}/messages/latest")]
		public IHttpActionResult GetLatestMessages(int roomId)
		{
			var room = GetRoomById(roomId) ?? GetPrivateRoomById(roomId);

			if (room == null)
			{
				return this.BadRequest("Invalid room id");
			}

			var messages = this.GetLatestMessages(DateTime.Now);

			if (messages == null)
			{
				return this.Ok("No new messages were found.");
			}
			return this.Ok(messages);
		}

		private IQueryable<MessageOutputModel> GetLatestMessages(DateTime date)
		{
			TimeSpan timeout = new TimeSpan(0, 0, 2, 0); // 2 minutes

			while (true)
			{
				var messages = this.data.Messages.All()
					.Where(m => m.Date > date)
					.Select(m => new MessageOutputModel()
					{
						Date = m.Date,
						SenderName = m.Sender.Name,
						Id = m.Id,
						Content = m.Content
					})
					.OrderByDescending(m => m.Date)
					.AsQueryable();

				if (messages.Any())
				{
					return messages;
				}
				else if (date - DateTime.Now > timeout)
				{
					return null;
				}

				Thread.Sleep(200);
			}
		}

		private Room GetRoomById(int id)
		{
			return this.data.PublicRooms.All().FirstOrDefault(r => r.Id == id);
		}

		private Room GetPrivateRoomById(int id)
		{
			return this.data.PrivateRooms.All().FirstOrDefault(r => r.Id == id);
		}
	}
}