using System;

namespace CinemaSystem.Models
{
    public class SeanceView
    {
        public int Id { get; set; }
        public DateTime ShowDate { get; set; }
        public TimeSpan ShowTime { get; set; }
        public CinemaView Cinema { get; set; }
        public HallView Hall { get; set; }
        public MovieView Movie { get; set; }
    }
}
