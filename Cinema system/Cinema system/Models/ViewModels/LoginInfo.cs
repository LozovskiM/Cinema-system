using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class LoginInfo
    {
        [Required(ErrorMessage = "Email cannot be empty")]
        [EmailAddress(ErrorMessage = "Wrong mail format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }
    }
}
