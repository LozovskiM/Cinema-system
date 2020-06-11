using System.Collections.Generic;

namespace CinemaSystem.Models
{
    public partial class Hall
    {
        public Hall()
        {
            Seats = new HashSet<Seat>();
            Seances = new HashSet<Seance>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CinemaId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Cinema Cinema { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
        public virtual ICollection<Seance> Seances { get; set; }
    }
}
