using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class OrderInfo
    {
        [Required(ErrorMessage = "Seance cannot be empty")]
        public int SeanceId { get; set; }

        [Required(ErrorMessage = "Seat cannot be empty")]
        public int SeatId { get; set; }

        [Required(ErrorMessage = "User cannot be empty")]
        public int UserId { get; set; }
    }
}
