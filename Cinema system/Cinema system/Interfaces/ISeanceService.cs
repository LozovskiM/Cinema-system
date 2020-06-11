using CinemaSystem.Models;
using System.Collections.Generic;

namespace CinemaSystem.Interfaces
{
    public interface ISeanceService
    {
        bool CheckHallExists(int hallId);
        bool CheckSeanceExists(SeanceInfo seance);
        bool CheckCorrectMovie(SeanceInfo seance);
        bool CheckSeanceExists(int seanceId);
        bool CheckCorrectSeanceTime(SeanceInfo seance);
        bool СheckForNonmatchingSeanceTime(SeanceInfo seance, int? seanceId = null);
        int CreateSeance(SeanceInfo seance);
        void EditSeance(int seanceId, SeanceInfo seance);
        void DeleteSeance(int seanceid);
        IEnumerable<SeanceView> GetSeances(SeanceFilter filter);
        public SeanceFullView GetSeance(int seanceId);

    }
}
