using System;
using System.Collections.Generic;

namespace CinemaSystem.Models
{
    public partial class Hall
    {
        public Hall()
        {
            Seats = new HashSet<Seat>();
        }

        public int Id { get; set; }
        public int CinemaId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Cinema Cinema { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
