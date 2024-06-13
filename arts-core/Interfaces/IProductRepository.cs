using arts_core.Data;
using arts_core.Models;
using arts_core.RequestModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace arts_core.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        public CustomResult CreateProduct(CreateProduct product);

    }

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(DataContext dataContext, ILogger<ProductRepository> logger) : base(dataContext)
        {
            _logger = logger;
        }

        public CustomResult CreateProduct(CreateProduct product)
        {
            foreach(var json in product.VariantDetailsJSON)
            {
                var detail = JsonConvert.DeserializeObject<VariantDetail>(json);
                product.VariantDetails.Add(detail);
            }

            foreach(var json in product.VariantsJSON)
            {
                var variant = JsonConvert.DeserializeObject<RequestModels.Variant>(json);
                product.Variants.Add(variant);
            }


            var newProduct = new Product()
            {
                CategoryId = product.Category,
                Name = product.ProductName,
                Description = product.Description,

            };

            foreach (dynamic variant in product.VariantDetails)
            {
                var newVariant = new Models.Variant() { Active = true };

                newVariant.Product = newProduct;

                _context.Variants.Add(newVariant);


                for (int i = 0; i < product.Variants.Count; i++)
                {
                    dynamic option = product.Variants.ToArray()[i];
                    var newVariantAttriBute = new VariantAttribute()
                    {

                        AttributeTypeId = option.Id,
                        AttributeValue = variant.Variant[i],
                    };
                    newVariantAttriBute.Variant = newVariant;
                    _context.VariantAttributes.Add(newVariantAttriBute);
                }
            }

            _context.Products.Add(newProduct);

            return new CustomResult(200, "success", newProduct);
        }

    
    }
}
