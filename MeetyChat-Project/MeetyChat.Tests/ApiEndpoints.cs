namespace MeetyChat.Tests
{
    public static class ApiEndpoints
    {
        #region api/Account
        public const string UserRegister = "/api/account/register";
        public const string UserLogin = "/api/account/login";
        public const string UserLogout = "/api/account/logout";
        #endregion

        #region api/profile
        public const string GetProfileInfo = "/api/profile";
        public const string EditProfileInfo = "/api/profile";
        public const string ChangePassword = "/api/profile/changepassword";
        #endregion

        #region api/rooms
        public const string GetRoomMessagesById = "/api/rooms/{0}/messages";
        public const string AddRoomMessage = "/api/rooms/{0}/messages";
        public const string GetRoomLatestMessagesById = "/api/rooms/{0}/messages/latest";

        public const string GetUsersByRoom = "/api/rooms/{0}/users";
        public const string GetLeftUsersByRoomId = "/api/rooms/{0}/users/latest/left";
        public const string GetLatestUsersByRoomId = "/api/rooms/{0}/users/latest/joined";
        public const string JointRoomById = "/api/rooms/{0}/join";
        public const string LeaveRoomById = "/api/rooms/{0}/leave";
        #endregion
    }
}
