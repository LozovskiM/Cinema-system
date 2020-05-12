using System;
using System.Collections.Generic;

namespace CinemaSystem.Models
{
    public partial class Seance
    {
        public Seance()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public DateTime ShowDate { get; set; }
        public TimeSpan ShowTime { get; set; }
        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
