using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class CinemaView
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title cannot be empty")]
        [StringLength(50,ErrorMessage = "Title must be less than 50 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "City cannot be empty")]
        [StringLength(50, ErrorMessage = "City must be less than 50 characters")]
        public string City { get; set; }
    }
}
