using System;
using Microsoft.AspNetCore.Identity;

namespace MyChat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime BirthDate { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsBlocked { get; set; }
    }
}