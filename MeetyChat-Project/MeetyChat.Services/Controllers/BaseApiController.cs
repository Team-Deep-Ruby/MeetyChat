namespace MeetyChat.Services.Controllers
{
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Microsoft.AspNet.Identity;

    public abstract class BaseApiController : ApiController
    {
        protected IMeetyChatData data;

        protected BaseApiController()
            : this(new MeetyChatData())
        {
        }

        protected BaseApiController(IMeetyChatData data)
        {
            this.data = data;
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
