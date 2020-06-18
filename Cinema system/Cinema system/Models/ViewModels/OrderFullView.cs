namespace CinemaSystem.Models
{
    public class OrderFullView
    {
        public int Id { get; set; }

        public SeanceView Seance { get; set; }

        public SeatView Seat { get; set; }
    }
}
