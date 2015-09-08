namespace MeetyChat.Models
{
    public class PrivateRoom : Room
    {
        public string FirstUserId { get; set; }

        public string SecondUserId { get; set; }
    }
}
