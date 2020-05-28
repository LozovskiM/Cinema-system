using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class MovieFullView
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title cannot be empty")]
        [StringLength(50, ErrorMessage = "Title must be less than 50 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Release date is required")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Ending date is required")]
        public DateTime EndingDate { get; set; }

        [Required(ErrorMessage = "Description cannot be empty")]
        [StringLength(256, ErrorMessage = "Description must be less than 256 characters")]
        public string Description { get; set; }
    }
}
