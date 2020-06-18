using CinemaSystem.Models;
using System.Collections.Generic;

namespace CinemaSystem.Interfaces
{
    public interface IOrderService
    {
        bool CheckUserExists(int userId);
        Order FindOrder(int userId, int orderId);
        bool CheckSeatBooked(OrderInfo order);
        bool CheckSeanceExists(OrderInfo order);
        int CreateOrder(OrderInfo order);
        IEnumerable<OrderView> GetOrders(int userId);
        OrderFullView GetOrder(Order order);
        void DeleteOrder(Order order);
    }
}
