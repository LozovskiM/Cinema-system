using CinemaSystem.Interfaces;
using CinemaSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaSystem.Services
{
    public class SeanceService : ISeanceService
    {
        private readonly CinemaDBContext _db;

        public SeanceService(CinemaDBContext db)
        {
            _db = db;
        }
        public bool CheckSeanceExists(int seanceId)
        {
            return _db.Seances.Any(s => (s.Id == seanceId) && !s.IsDeleted);
        }

        public bool CheckHallExists(int hallId)
        {
            return _db.Halls.Any(r => r.Id == hallId);
        }

        public bool CheckSeanceExists(SeanceInfo seance)
        {
            return _db.Seances.Any(s =>
                s.HallId == seance.HallId
                && s.ShowTime == seance.ShowTime
                && s.ShowDate == seance.ShowDate
                && s.MovieId == seance.MovieId
                && !s.IsDeleted
            );
        }

        public bool CheckCorrectMovie(SeanceInfo seance)
        {
            return _db.Movies
                .Any(m => m.Id == seance.MovieId
                && m.ReleaseDate <= seance.ShowDate
                && seance.ShowDate <= m.EndingDate);
        }

        public bool CheckCorrectSeanceTime(SeanceInfo seance)
        {
            return seance.ShowDate.Date > DateTime.Now.Date;
        }

        public bool СheckForNonmatchingSeanceTime(SeanceInfo seance, int? seanceId = null)
        {
            var prevSeance = _db.Seances
                .Where(s => s.HallId == seance.HallId
                   && seance.ShowDate.Date == s.ShowDate.Date
                   && s.ShowDate < seance.ShowDate
                   && s.Id != seanceId)
                .OrderBy(s => s.ShowDate)
                .LastOrDefault();

            if (prevSeance == null)
            {
                return true;
            }

            if (seance.ShowDate < prevSeance.ShowDate.Add(prevSeance.ShowTime))
            {
                return false;
            }

            var nextSeance = _db.Seances
                .Where(s => s.HallId == seance.HallId
                   && seance.ShowDate.Date == s.ShowDate.Date
                   && s.ShowDate > seance.ShowDate)
                .OrderBy(s => s.ShowDate)
                .FirstOrDefault();

            if (nextSeance == null)
            {
                return true;
            }

            if (nextSeance.ShowDate < seance.ShowDate.Add(seance.ShowTime))
            {
                return false;
            }

            return true;
        }

        public int CreateSeance(SeanceInfo seance)
        {
            Seance newSeance = new Seance
            {
                MovieId = seance.MovieId,
                HallId = seance.HallId,
                ShowDate = seance.ShowDate,
                ShowTime = seance.ShowTime
            };

            foreach(SeanceSeatView seanceSeat in seance.SessionSeats)
            {
                var newSeanceSeat = new SeanceSeat
                {
                    SeanceId = newSeance.Id,
                    SeatId = seanceSeat.SeatId,
                    Price = seanceSeat.Price,
                    IsBooked = seanceSeat.IsBooked
                };

                _db.SeanceSeats.Add(newSeanceSeat);
            }

            _db.Seances.Add(newSeance);

            _db.SaveChanges();

            return newSeance.Id;
        }

        public void EditSeance(int seanceId, SeanceInfo seance)
        {
            Seance editSeance = _db.Seances.Find(seanceId);

            editSeance.ShowDate = seance.ShowDate;
            editSeance.ShowTime = seance.ShowTime;
            editSeance.MovieId = seance.MovieId;
            editSeance.HallId = seance.HallId;
            editSeance.ShowTime = seance.ShowTime;

            var seanceSeat = _db.SeanceSeats
                .Where(s => s.SeanceId == editSeance.Id);

            foreach (SeanceSeat editSeanceSeat in seanceSeat)
            {
                var foundSeanceSeat = seance.SessionSeats
                    .Where(s => s.SeatId == editSeanceSeat.SeatId)
                    .SingleOrDefault();

                if (foundSeanceSeat != null)
                {
                    editSeanceSeat.Price = foundSeanceSeat.Price;
                    editSeanceSeat.IsBooked = foundSeanceSeat.IsBooked;
                }
            }

            _db.SaveChanges();
        }

        public IEnumerable<SeanceView> GetSeances(SeanceFilter filter)
        {
            IQueryable<Seance> seances = _db.Seances
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .Include(s => s.Hall.Cinema)
                .AsNoTracking();

            if (filter.MovieId != null)
            {
                seances = seances.Where(s => s.MovieId == filter.MovieId);
            }
            if (filter.StartDate != null)
            {
                seances = seances.Where(s => filter.StartDate < s.ShowDate);
            }
            if (filter.EndDate != null)
            {
                seances = seances.Where(s => s.ShowDate < filter.EndDate);
            }

            return seances.Where(s => !s.IsDeleted)
                .Select(sc => new SeanceView
                {
                    Id = sc.Id,
                    ShowDate = sc.ShowDate,
                    ShowTime = sc.ShowTime,
                    Cinema = new CinemaView
                    {
                        Id = sc.Hall.Cinema.Id,
                        Title = sc.Hall.Cinema.Title,
                        City = sc.Hall.Cinema.City
                    },
                    Hall = new HallView
                    {
                        Id = sc.HallId,
                        Name = sc.Hall.Name
                    },
                    Movie = new MovieView
                    {
                        Id = sc.MovieId,
                        Title = sc.Movie.Title
                    }
                });
        }

        public SeanceFullView GetSeance(int seanceId)
        {
            var seance = _db.Seances.Find(seanceId);

            return new SeanceFullView
            {
                Id = seance.Id,
                ShowDate = seance.ShowDate,
                ShowTime = seance.ShowTime,
                Hall = new HallView
                {
                    Id = seance.HallId,
                    Name = seance.Hall.Name
                },
                Cinema = new CinemaView
                {
                    Id = seance.Hall.CinemaId,
                    Title = seance.Hall.Cinema.Title,
                    City = seance.Hall.Cinema.City
                },
                SessionSeats = _db.SeanceSeats
                    .Where(s => s.SeanceId == seanceId)
                    .Select(st => new SeanceSeatView
                    {
                        SeatId = st.SeatId,
                        Price = st.Price,
                        IsBooked = st.IsBooked
                    })
            };
        }

        public void DeleteSeance(int seanceId)
        {
            var deleteSeance = _db.Seances.Find(seanceId);

            deleteSeance.IsDeleted = true;

            _db.SaveChanges();
        }
    }
}
