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

        bool CheckCinemaExists(int cinemaId);

        bool CheckCinemaExists(CinemaView cinema);

        int CreateHall(int cinemaId, HallFullView hall);

        HallFullView GetHall(int hallId);

        IEnumerable<HallView> GetHalls(int cinemaId);

        void EditHall(int hallId, HallFullView hall);

        void DeleteHall(int hallId);

        bool CheckHallExists(int cinemaId, int hallId);

        bool CheckHallExists(int cinemaId, HallFullView hall);
    }
}
