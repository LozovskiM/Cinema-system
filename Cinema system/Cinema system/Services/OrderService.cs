using CinemaSystem.Interfaces;
using CinemaSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace CinemaSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly CinemaDBContext _db;

        public OrderService(CinemaDBContext db)
        {
            _db = db;
        }

        public bool CheckUserExists(int userId)
        {
            return _db.Users.Any(u => u.Id == userId);
        }

        public Order FindOrder(int userId, int orderId)
        {
            return _db.Orders.FirstOrDefault(o => (o.Id == orderId && o.UserId == userId && !o.IsDeleted));
        }
        public bool CheckSeatBooked(OrderInfo order)
        {
            return _db.SeanceSeats.Any(s => (!s.IsBooked && s.SeanceId == order.SeanceId && s.SeatId == order.SeatId));
        }

        public bool CheckSeanceExists(OrderInfo order)
        {
            return _db.Seances.Any(s => (s.Id == order.SeanceId && !s.IsDeleted));
        }

        public IEnumerable<OrderView> GetOrders(int userId)
        {
            return _db.Orders
                .Where(o => (o.UserId == userId && !o.IsDeleted))
                .Select(or => new OrderView
                {
                    Id = or.Id,
                    Seance = new SeanceView
                    {
                        Id = or.SeanceId,
                        ShowDate = or.Seance.ShowDate,
                        ShowTime = or.Seance.ShowTime,
                        Cinema = new CinemaView
                        {
                            Id = or.Seance.Hall.Cinema.Id,
                            Title = or.Seance.Hall.Cinema.Title,
                            City = or.Seance.Hall.Cinema.City
                        },
                        Hall = new HallView
                        {
                            Id = or.Seance.HallId,
                            Name = or.Seance.Hall.Name
                        },
                        Movie = new MovieView
                        {
                            Id = or.Seance.MovieId,
                            Title = or.Seance.Movie.Title
                        }
                    }
                });
        }

        public OrderFullView GetOrder(Order order)
        {
            return new OrderFullView
            {
                Id = order.Id,
                Seance = new SeanceView
                {
                    Id = order.SeanceId,
                    ShowDate = order.Seance.ShowDate,
                    ShowTime = order.Seance.ShowTime,
                    Cinema = new CinemaView
                    {
                        Id = order.Seance.Hall.Cinema.Id,
                        Title = order.Seance.Hall.Cinema.Title,
                        City = order.Seance.Hall.Cinema.City
                    },
                    Hall = new HallView
                    {
                        Id = order.Seance.HallId,
                        Name = order.Seance.Hall.Name
                    },
                    Movie = new MovieView
                    {
                        Id = order.Seance.MovieId,
                        Title = order.Seance.Movie.Title
                    }
                },
                Seat = new SeatView
                {
                    Id = order.SeatId,
                    Row = order.Seat.Row,
                    Place = order.Seat.Place,
                    TypeOfSeat = order.Seat.TypeOfSeat.ToString()
                }
            };
        }

        public int CreateOrder(OrderInfo order)
        {
            Order newOrder = new Order
            {
                UserId = order.UserId,
                SeanceId = order.SeanceId,
                SeatId = order.SeatId
            };

            _db.Orders.Add(newOrder);

            var seanceSeat = _db.SeanceSeats
                .Where(s => (s.SeanceId == newOrder.SeanceId && s.SeatId == newOrder.SeatId))
                .SingleOrDefault();

            seanceSeat.IsBooked = true;

            _db.SaveChanges();

            return newOrder.Id;
        }

        public void DeleteOrder(Order order)
        {
            order.IsDeleted = true;

            _db.SaveChanges();
        }
    }
}