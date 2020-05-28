using System;
using System.Collections.Generic;

namespace CinemaSystem.Models
{
    public partial class Movie
    {
        public Movie()
        {
            Seances = new HashSet<Seance>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime EndingDate { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Seance> Seances { get; set; }
    }
}
