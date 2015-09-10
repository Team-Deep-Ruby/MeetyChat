namespace MeetyChat.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Data.Interfaces;
    using UserSessionUtils;

    [SessionAuthorize]
    public class UsersController : BaseApiController
    {
        public UsersController(IMeetyChatData data)
            : base(data)
        {
        }

        [HttpGet]
        [Route("api/users/active")]
        public IHttpActionResult GetAllActiveUsers()
        {
            var activeUsers = this.data.UserSessions.All()
                .Select(us => new
                {
                    us.OwnerUser.UserName,
                    us.OwnerUser.Name
                });

            return this.Ok(activeUsers);
        }
    }
}
