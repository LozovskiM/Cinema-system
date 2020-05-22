using CinemaSystem.Models;
using System.Collections.Generic;

namespace CinemaSystem.Interfaces
{
    public interface ICinemaService
    {
        int CreateCinema(CinemaView cinema);

        CinemaView GetCinema(int cinemaId);

        IEnumerable<CinemaView> GetCinemas();

        void EditCinema(int cinemaId, CinemaView cinema);

        void DeleteCinema(int cinemaId);

        bool IsCinemaExists(int cinemaId);

        bool IsCinemaExists(CinemaView cinema);
    }
}
