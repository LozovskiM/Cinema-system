using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class SeanceInfo
    {
        [Required(ErrorMessage = "Show date cannot be empty")]
        public DateTime ShowDate { get; set; }

        [Required(ErrorMessage = "Show time cannot be empty")]
        public TimeSpan ShowTime { get; set; }

        [Required(ErrorMessage = "Movie cannot be empty")]
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Hall cannot be empty")]
        public int HallId { get; set; }

        [Required(ErrorMessage = "Seance cannot be without seats (at least one)")]
        public IEnumerable<SeanceSeatView> SessionSeats { get; set; }
    }
}
