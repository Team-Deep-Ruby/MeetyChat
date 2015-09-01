namespace MeetyChat.Models
{
    using System;

    public class RoomsJoiningHistory
    {
        public int Id { get; set; }

        public ApplicationUser User { get; set; }

        public Room Room { get; set; }

        public DateTime JoinedOn { get; set; }

        public DateTime? LeftOn { get; set; }
    }
}