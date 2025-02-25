using ExampleProject4.Core.Entities;
using ExampleProject4.Infrastructure;
using ExampleProject4.WebApi.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace ExampleProject4.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController: ControllerBase
    {
        public ExampleDbContext DbContext { get; set; }

        public ProductsController()
        {
            DbContext = new ExampleDbContext();
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = DbContext.Products.ToList();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            Product? product = DbContext.Products.FirstOrDefault(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct(CreateProductModel model)
        {
            Product product = new Product
            {
                Name = model.Name,
                Rating = model.Rating
            };

            DbContext.Products.Add(product);
            DbContext.SaveChanges();

            return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"UpdateProduct {id}");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            Product? product = DbContext.Products.FirstOrDefault(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }

            DbContext.Products.Remove(product);
            DbContext.SaveChanges();

            return Ok();
        }

    }
}
