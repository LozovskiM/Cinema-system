using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CinemaSystem.Interfaces;
using CinemaSystem.Models;

namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IOrderService _orderService;

        private const string ErrorOfUserNonexistence = "This user does not exists.";
        private const string ErrorOfOrderNonexistence = "This order does not exists.";
        private const string ErrorOfSeanceNonexistence = "This seance does not exists.";
        private const string ErrorOfSeatBooked = "This seat does not exists or has already booked";

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{userId}")]
        public IActionResult GetOrders(int userId)
        {
            if (!_orderService.CheckUserExists(userId))
            {
                return NotFound(new Response(ErrorOfUserNonexistence));
            }

            var orders = _orderService.GetOrders(userId);

            return Ok(new GetResponse<IEnumerable<OrderView>>(orders));
        }

        [HttpGet("{userId}/user/{orderId}")]
        public IActionResult GetOrder(int userId, int orderId)
        {
            if (!_orderService.CheckUserExists(userId))
            {
                return NotFound(new Response(ErrorOfUserNonexistence));
            }

            var findOrder = _orderService.FindOrder(userId, orderId);

            if (findOrder == null)
            {
                return NotFound(new Response(ErrorOfOrderNonexistence));
            }

            var order = _orderService.GetOrder(findOrder);

            return Ok(new GetResponse<OrderFullView>(order));
        }

        [HttpPost("{userId}/user")]
        public IActionResult CreateOrder([FromBody] OrderInfo order, int userId)
        {
            if (!_orderService.CheckUserExists(userId))
            {
                return NotFound(new Response(ErrorOfUserNonexistence));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new Response(ModelState));
            }

            if (!_orderService.CheckSeanceExists(order))
            {
                return NotFound(new Response(ErrorOfSeanceNonexistence));
            }

            if (!_orderService.CheckSeatBooked(order))
            {
                return NotFound(new Response(ErrorOfSeatBooked));
            }

            var id = _orderService.CreateOrder(order);

            var response = new CreateResponse(id);

            return CreatedAtAction(nameof(GetOrder), new { userId, orderId = response.Id }, response);
        }

        [HttpDelete("{userId}/user/{orderId}")]
        public IActionResult DeleteOrder(int userId, int orderId)
        {
            if (!_orderService.CheckUserExists(userId))
            {
                return NotFound(new Response(ErrorOfUserNonexistence));
            }

            var findOrder = _orderService.FindOrder(userId, orderId);

            if (findOrder == null)
            {
                return NotFound(new Response(ErrorOfOrderNonexistence));
            }

            _orderService.DeleteOrder(findOrder);

            return Ok(new Response());
        }
    }
}