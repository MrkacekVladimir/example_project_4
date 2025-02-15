﻿using ExampleProject4.Core.Entities;
using ExampleProject4.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExampleProject4.WebApi.Controllers
{
    [ApiController]
    [Route("/api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly ExampleDbContext dbContext;

        public OrdersController(ExampleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<Order>> GetAllOrders()
        {
            List<Order> orders = dbContext.Orders.ToList();

            return orders;
        }

        [HttpGet("{orderId:int}/items")]
        public ActionResult<List<OrderItem>> GetOrderItemsByOrderId(int orderId)
        {
            List<OrderItem> orderItems = dbContext.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToList();

            return orderItems;
        }



    }
}
