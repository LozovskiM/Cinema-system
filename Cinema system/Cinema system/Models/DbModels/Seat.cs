namespace CinemaSystem.Models
{
    public enum SeatType
    {
        VIP,
        Usual,
        Double
    }

    public partial class Seat
    {
        public int Id { get; set; }
        public SeatType TypeOfSeat { get; set; }
        public int Row { get; set; }
        public int Place { get; set; }
        public int HallId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Hall Hall { get; set; }
    }
}
