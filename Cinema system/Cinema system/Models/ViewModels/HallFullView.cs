using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class HallFullView
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name cannot be empty")]
        [StringLength(50, ErrorMessage = "Name must be less than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Hall cannot be without seats (at least one)")]
        public IEnumerable<SeatView> Seats { get; set; }
    }
}
