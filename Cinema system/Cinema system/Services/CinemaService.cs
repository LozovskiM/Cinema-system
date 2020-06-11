using CinemaSystem.Interfaces;
using CinemaSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

        public bool CheckCinemaExists(int cinemaId)
        {
            return _db.Cinemas.Any(c => (c.Id == cinemaId && !c.IsDeleted));
        }

        public bool CheckCinemaExists(CinemaView cinema)
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

        public bool CheckHallExists(int cinemaId, int hallId)
        {
            return _db.Halls.Any(h => (h.Id == hallId && h.CinemaId == cinemaId && !h.IsDeleted));
        }

        public bool CheckHallExists(int cinemaId, HallInfo hall)
        {
            return _db.Halls.Any(h => (h.Name == hall.Name && h.CinemaId == cinemaId && !h.IsDeleted));
        }

        public int CreateHall(int cinemaId, HallInfo hall)
        {
            var newHall = new Hall
            {
                Name = hall.Name,
                Seats = hall.Seats
                    .Select(s => new Seat
                    {
                        TypeOfSeat = (SeatType)Enum.Parse(typeof(SeatType), s.TypeOfSeat),
                        Row = s.Row,
                        Place = s.Place
                    })
                    .ToList()
            };

            _db.Halls.Add(newHall);

            _db.SaveChanges();

            return newHall.Id;
        }

        public void EditHall(int hallId, HallInfo hall)
        {
            var editHall = _db.Halls
                .Include(h => h.Seats)
                .SingleOrDefault(h => h.Id == hallId);

            editHall.Name = hall.Name;

            editHall.Seats = hall.Seats
                .Select(s => new Seat
                {
                    TypeOfSeat = (SeatType)Enum.Parse(typeof(SeatType), s.TypeOfSeat),
                    Row = s.Row,
                    Place = s.Place
                })
                .ToList();

            _db.SaveChanges();
        }

        public IEnumerable<HallView> GetHalls(int cinemaId)
        {
            return _db.Halls
                .Where(h => (h.CinemaId == cinemaId && !h.IsDeleted))
                .Select(h => new HallView
                {
                    Id = h.Id,
                    Name = h.Name,
                });
        }

        public HallFullView GetHall(int hallId)
        {
            var hall = _db.Halls.Find(hallId);

            return new HallFullView
            {
                Id = hall.Id,
                Name = hall.Name,
                Seats = hall.Seats
                    .Select(s => new SeatView
                    {
                        Id = s.Id,
                        TypeOfSeat = s.TypeOfSeat.ToString(),
                        Row = s.Row,
                        Place = s.Place
                    })
            };
        }

        public void DeleteHall(int hallId)
        {
            var deleteHall = _db.Halls.Find(hallId);

            deleteHall.IsDeleted = true;

            _db.SaveChanges();
        }
    }
}
