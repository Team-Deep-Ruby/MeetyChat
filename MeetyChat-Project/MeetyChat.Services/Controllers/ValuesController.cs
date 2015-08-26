namespace MeetyChat.Services.Controllers
{
    using System.Web.Http;
    using Data.Interfaces;
    using Infrastructure;

    public class ValuesController : BaseApiController
    {
        private IUserIdProvider provider;

        public ValuesController(IMeetyChatData data, IUserIdProvider provider) 
            : base(data)
        {
            this.provider = provider;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return this.Ok();
        }
    }
}