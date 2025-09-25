using System.ComponentModel.DataAnnotations;

namespace MyChat.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username или Email")]
        public string UserNameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}