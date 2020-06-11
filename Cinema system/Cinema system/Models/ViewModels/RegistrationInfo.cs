using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class RegistrationInfo
    {
        [Required(ErrorMessage = "Email cannot be empty")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First name cannot be empty")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name cannot be empty")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "User name cannot be empty")]
        public string UserName { get; set; }
    }
}
