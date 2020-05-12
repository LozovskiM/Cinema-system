﻿using System;
using System.Collections.Generic;

namespace CinemaSystem.Models
{
    public partial class SeanceSeat
    {
        public int SeanceId { get; set; }
        public int SeatId { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        public virtual Seance Seance { get; set; }
        public virtual Seat Seat { get; set; }
    }
}
