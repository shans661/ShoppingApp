using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        public StoreContext Context { get; set; }
        public ProductsController(StoreContext context)
        {
            this.Context = context;
        }
        [HttpGet]
        public ActionResult<List<Product>> GetProducts()
        {
            var products = this.Context.Products.ToList();

            return products;
        }
        [HttpGet("{id}")]
        public string GetProducts(int id)
        {
            return "this is list of products";
        }

    }
}