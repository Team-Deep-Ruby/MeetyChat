namespace MeetyChat.Services.Controllers
{
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Microsoft.AspNet.Identity;

    public abstract class BaseApiController : ApiController
    {
        protected IMeetyChatData data;
        protected const int ProfileImageKbLimit = 128;

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

        protected bool ValidateImageSize(string imageDataUrl, int kbLimit)
        {
            // Image delete
            if (imageDataUrl == null)
            {
                return true;
            }

            // Every 4 bytes from Base64 is equal to 3 bytes
            if ((imageDataUrl.Length / 4) * 3 >= kbLimit * 1024)
            {
                return false;
            }

            return true;
        }
    }
}
