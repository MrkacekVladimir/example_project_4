using ExampleProject4.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExampleProject4.WebApi.Controllers
{
    public class OrdersController : Controller
    {
        [HttpGet("/orders")]
        public IActionResult GetAllOrders()
        {
            //Vytvářím fake data
            List<Order> orders = new List<Order>();
            orders.Add(new Order() { Id = 1, CreatedOn = DateTime.Now});
            orders.Add(new Order() { Id = 2, CreatedOn = DateTime.Now});
            orders.Add(new Order() { Id = 3, CreatedOn = DateTime.Now});

            return Json(orders);
        }
    }
}
