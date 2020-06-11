using System;
using System.Collections.Generic;

namespace CinemaSystem.Models
{
    public class SeanceFullView
    {
        public int Id { get; set; }
        public DateTime ShowDate { get; set; }
        public TimeSpan ShowTime { get; set; }
        public CinemaView Cinema { get; set; }
        public HallView Hall { get; set; }
        public MovieView Movie { get; set; }

        public IEnumerable<SeanceSeatView> SessionSeats { get; set; }
    }
}
