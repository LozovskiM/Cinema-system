using System.Collections.Generic;

namespace CinemaSystem.Models
{
    public class HallFullView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<SeatView> Seats { get; set; }
    }
}
