using Microsoft.AspNetCore.Identity;
using System;

namespace MyChat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime? BirthDate { get; set; }  
        public string? AvatarUrl { get; set; }  
        public bool IsBlocked { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Avatar { get; set; }
    }
}