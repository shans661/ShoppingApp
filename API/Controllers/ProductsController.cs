using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using API.Helpers;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
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
          public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetProducts([FromQuery]ProductSpecParams productParams)
        {
            ProductsWithTypesAndBrandsSpecification spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            ProductWithFiltersForCountSpecification countSpec = new ProductWithFiltersForCountSpecification(productParams);

            int totalItems = await ProductRepo.CountAsync(spec);

            var products = await this.ProductRepo.ListAsync(spec);

            IReadOnlyList<ProductDTO> data = Mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(products);
            return Ok(new Pagination<ProductDTO>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetProducts(int id)
        {
            ProductsWithTypesAndBrandsSpecification spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await this.ProductRepo.GetEntityWithSpec(spec);

            if(product == null) return NotFound(new ApiResponse(404));
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