namespace MeetyChat.Services.Controllers
{
    using System.Web.Http;
    using Data.Interfaces;

    public abstract class BaseApiController : ApiController
    {
        protected IMeetyChatData data;

        protected BaseApiController(IMeetyChatData data)
        {
            this.data = data;
        }
    }
}
