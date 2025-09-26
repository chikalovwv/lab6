using System;

namespace MyChat.Models
{
    public class Message
    {
        public string Text { get; set; } = string.Empty; 
        public string? UserName { get; set; }   
        public string? AvatarUrl { get; set; }          
        public int Id { get; set; }

        public DateTime SentAt { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}