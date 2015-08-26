namespace MeetyChat.Services.Models.InputModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MessageInputModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string SenderId { get; set; }
    }
}