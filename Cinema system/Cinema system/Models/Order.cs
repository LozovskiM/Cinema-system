using System;
using System.Collections.Generic;

namespace CinemaSystem.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public int SeanceId { get; set; }
        public int SeatId { get; set; }

        public virtual Seance Seance { get; set; }
        public virtual Seat Seat { get; set; }
    }
}
