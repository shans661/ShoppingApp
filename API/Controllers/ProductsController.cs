using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> ProductRepo;
        private readonly IGenericRepository<ProductBrand> ProductBrandRepo;
        private readonly IGenericRepository<ProductType> ProductTypeRepo;
        private readonly IMapper Mapper;

        public ProductsController(IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandRepo,
        IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            this.Mapper = mapper;
            this.ProductRepo = productRepo;
            this.ProductBrandRepo= productBrandRepo;
            this.ProductTypeRepo = productTypeRepo;
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

            return Mapper.Map<Product, ProductDTO>(product);
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