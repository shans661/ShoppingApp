using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private StoreContext Context;
        public ProductRepository(StoreContext context)
        {
            this.Context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await Context.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            Product product = await Context.Products.FindAsync(id);

            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await Context.Products.ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await Context.ProductTypes.ToListAsync();
        }
    }
}