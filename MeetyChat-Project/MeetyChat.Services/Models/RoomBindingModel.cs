namespace MeetyChat.Services.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class RoomBindingModel
    {
        [Required]
        [MinLength(4)]
        public string Name { get; set; }
    }
}