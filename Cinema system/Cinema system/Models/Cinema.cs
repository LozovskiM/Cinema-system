using System;
using System.Collections.Generic;

namespace CinemaSystem.Models
{
    public partial class Cinema
    {
        public Cinema()
        {
            Halls = new HashSet<Hall>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Hall> Halls { get; set; }
    }
}
