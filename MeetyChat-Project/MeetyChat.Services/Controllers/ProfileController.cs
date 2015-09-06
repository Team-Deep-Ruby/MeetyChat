namespace MeetyChat.Services.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Infrastructure;
    using MeetyChat.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.Users;
    using UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/Profile")]
    public class ProfileController : BaseApiController
    {
        private readonly ApplicationUserManager userManager;
        private readonly IUserIdProvider provider;

        public ProfileController()
        {
            this.userManager = new ApplicationUserManager(
                new UserStore<ApplicationUser>(new MeetyChatDbContext()));
        }

        public ProfileController(IMeetyChatData data, 
            IUserIdProvider provider) 
            : base(data)
        {
            this.provider = provider;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager;
            }
        }

        [HttpGet]
        public IHttpActionResult GetProfileInfo()
        {
            var userId = this.provider.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var user = this.data.Users.All()
                .FirstOrDefault(u => u.Id == userId);

            return this.Ok(new ProfileViewModel()
            {
                Id = user.Id,
                Username = user.UserName,
                Name = user.Name,
                Email = user.Email,
                ProfileImage = user.ProfileImage,
                Gender = user.Gender
            });
        }

        [HttpPut]
        public IHttpActionResult EditProfileInfo(EditUserBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            // Validate the current user exists in the database
            var currentUserId = this.provider.GetUserId();
            var currentUser = this.data.Users.All()
                .FirstOrDefault(u => u.Id == currentUserId);
            if (currentUser == null)
            {
                return this.BadRequest("Invalid user token.");
            }

            var emailHolder = this.data.Users.All()
                .FirstOrDefault(u => u.Email == model.Email);
            if (emailHolder != null && emailHolder.Id != currentUserId)
            {
                return this.BadRequest("Email is already taken.");
            }

            if (!this.ValidateImageSize(model.ProfileImage, ProfileImageKbLimit))
            {
                return this.BadRequest(string.Format("Profile image size should be less than {0}kb.", ProfileImageKbLimit));
            }

            currentUser.Name = model.Name;
            currentUser.Email = model.Email;
            currentUser.Gender = model.Gender;
            currentUser.ProfileImage = model.ProfileImage;

            this.data.SaveChanges();

            return this.Ok(new
            {
                message = "User profile edited successfully."
            });
        }

        // POST api/Profile/ChangePassword
        [HttpPut]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangeUserPassword(ChangePasswordBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this.UserManager.ChangePasswordAsync(
                this.User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok(new
                {
                    message = "Password successfully changed.",
                }
            );
        }
    }
}
