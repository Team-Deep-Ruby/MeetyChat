namespace MeetyChat.Services.Controllers
{
    using System;
    using System.Web.Http;
    using Data.Interfaces;
    using Infrastructure;

    public class MessagesController : BaseApiController
    {
        private IUserIdProvider userIdProvider;

        public MessagesController(IMeetyChatData data, IUserIdProvider userIdProvider)
            : base(data)
        {
            this.userIdProvider = userIdProvider;
        }

        [Route("api/rooms/{roomId}/messages")]
        public IHttpActionResult GetAllMessages(string roomId)
        {
            Guid roomGuid;
            try
            {
                roomGuid = Guid.Parse(roomId);
            }
            catch (FormatException)
            {
                return this.BadRequest("Invalid room id.");
            }
            
            return this.Ok("asdasdas");

        }
    }
}