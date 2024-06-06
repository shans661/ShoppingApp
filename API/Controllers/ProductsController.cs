using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> ProductRepo;
        public ProductsController(IGenericRepository<Product> productRepo)
        {
            this.ProductRepo = productRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetProducts()
        {
            ProductsWithTypesAndBrandsSpecification spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await this.ProductRepo.ListAsync(spec);
            return products.Select(x => new ProductDTO(){
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                PictureUrl = x.PictureUrl,
                Price = x.Price,
                ProductBrand = x.ProductBrand.Name,
                ProductType = x.ProductType.Name
            }).ToList();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProducts(int id)
        {
            ProductsWithTypesAndBrandsSpecification spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await this.ProductRepo.GetEntityWithSpec(spec);

            return new ProductDTO(){
                Id = product.Id,
                Description = product.Description,
                Name = product.Name,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            } ;
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(){
            return Ok(await ProductRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(){
            return Ok(await ProductRepo.ListAllAsync());
        }

    }
}