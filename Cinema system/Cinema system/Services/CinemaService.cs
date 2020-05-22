using CinemaSystem.Interfaces;
using CinemaSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace CinemaSystem.Services
{
    public class CinemaService : ICinemaService
    {

        private readonly CinemaDBContext _db;

        public CinemaService(CinemaDBContext db)
        {
            _db = db;
        }

        public bool IsCinemaExists(int cinemaId)
        {
            return _db.Cinemas.Any(c => (c.Id == cinemaId && !c.IsDeleted));
        }

        public bool IsCinemaExists(CinemaView cinema)
        {
            return _db.Cinemas.Any(c => (c.Title == cinema.Title && c.City == cinema.City && !c.IsDeleted));
        }

        public int CreateCinema(CinemaView cinema)
        {
            var newCinema = new Cinema
            {
                Title = cinema.Title,
                City = cinema.City
            };

            _db.Cinemas.Add(newCinema);
            _db.SaveChanges();

            return newCinema.Id;
        }

        public void EditCinema(int cinemaId, CinemaView cinema)
        {
            var editCinema = _db.Cinemas.Find(cinemaId);

            editCinema.Title = cinema.Title;
            editCinema.City = cinema.City;

            _db.SaveChanges();
        }

        public IEnumerable<CinemaView> GetCinemas()
        {
            return _db.Cinemas
                .Where(c => c.IsDeleted == false)
                .Select(c => new CinemaView
                {
                    Id = c.Id,
                    Title = c.Title,
                    City = c.City
                });
        }

        public CinemaView GetCinema(int cinemaId)
        {
            var cinema = _db.Cinemas.Find(cinemaId);

            return new CinemaView
            {
                Id = cinema.Id,
                Title = cinema.Title,
                City = cinema.City
            };   
        }

        public void DeleteCinema(int cinemaId)
        {
            var deleteCinema = _db.Cinemas.Find(cinemaId);

            deleteCinema.IsDeleted = true;

            _db.SaveChanges();
        }
    }
}
