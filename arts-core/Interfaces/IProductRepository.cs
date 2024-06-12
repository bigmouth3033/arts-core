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



            return new CustomResult(200, "success", product);
        }
    }
}
