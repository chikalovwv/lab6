using System;

namespace MyChat.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public string Text { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}