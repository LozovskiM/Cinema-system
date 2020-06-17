using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Email cannot be empty")]
        [StringLength(50, ErrorMessage = "Email must be less than 50 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        [StringLength(16, ErrorMessage = "Password must be less than 16 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role cannot be empty")]
        public string Role { get; set; }

        [Required(ErrorMessage = "UserName cannot be empty")]
        [StringLength(50, ErrorMessage = "User name must be less than 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "FirstName cannot be empty")]
        [StringLength(50, ErrorMessage = "First name must be less than 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName cannot be empty")]
        [StringLength(50, ErrorMessage = "Last name must be less than 50 characters")]
        public string LastName { get; set; }  
    }
}
