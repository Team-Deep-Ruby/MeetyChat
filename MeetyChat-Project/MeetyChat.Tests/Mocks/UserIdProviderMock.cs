namespace MeetyChat.Tests.Mocks
{
    using Moq;
    using Services.Infrastructure;

    class UserIdProviderMock
    {
        public static Mock<IUserIdProvider> GetUserIdProvider()
        {
            var userIdProviderMock = new Mock<IUserIdProvider>();

            userIdProviderMock.Setup(x => x.GetUserId()).
                Returns(MeetyChatDataMock.MockUserId);

            return userIdProviderMock;
        }
    }
}