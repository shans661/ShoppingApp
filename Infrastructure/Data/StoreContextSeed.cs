using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {
                string brands = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");

                List<ProductBrand> brandsData = JsonSerializer.Deserialize<List<ProductBrand>>(brands);
                context.ProductBrands.AddRange(brandsData);
            }

            if (!context.ProductTypes.Any())
            {
                string types = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");

                List<ProductType> typesData = JsonSerializer.Deserialize<List<ProductType>>(types);
                context.ProductTypes.AddRange(typesData);
            }

            if (!context.Products.Any())
            {
                string products = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");

                List<Product> productsData = JsonSerializer.Deserialize<List<Product>>(products);
                context.Products.AddRange(productsData);
            }

            if(context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();
        }
    }
}