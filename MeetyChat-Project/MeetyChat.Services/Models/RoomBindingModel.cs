namespace MeetyChat.Services.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class RoomBindingModel
    {
        [Required]
        [Index("IX_Name", IsUnique = true)]
        [MinLength(4)]
        public string Name { get; set; }
    }
}