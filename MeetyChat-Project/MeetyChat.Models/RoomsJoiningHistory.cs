namespace MeetyChat.Models
{
    using System;

    public class RoomsJoiningHistory
    {
        public ApplicationUser User { get; set; }

        public Room Room { get; set; }

        public DateTime JoinedOn { get; set; }
    }
}