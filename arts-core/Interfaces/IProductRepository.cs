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
        private readonly IWebHostEnvironment _env;
        public ProductRepository(DataContext dataContext, ILogger<ProductRepository> logger, IWebHostEnvironment env) : base(dataContext)
        {
            _logger = logger;
            _env = env;
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

            ICollection<string> imageNameList = new List<string>();

            foreach (var image in product.Images)
            {
                var fileName = DateTime.Now.Ticks + image.FileName;
                imageNameList.Add(fileName);
                var uploadPath = Path.Combine(_env.WebRootPath, "images");
                var filePath = Path.Combine(uploadPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                image.CopyTo(stream);

                var newImage = new ProductImage
                {
                    ImageName = fileName,
                    Product = newProduct
                };

                _context.ProductImages.Add(newImage);
            }

            foreach (dynamic variant in product.VariantDetails)
            {
                var newVariant = new Models.Variant() {
                    Active = true,
                    VariantImage = variant.Image != null ? imageNameList.ToArray()[variant.Image] : null,
                    Price = variant.SellPrice,
                    SalePrice = variant.ComparePrice,
                };

                var stock = new Stock
                {
                    CostPerItem = variant.BeginFund,
                    Quantity = variant.Inventory,
                    Variant = newVariant
                };

                newVariant.Product = newProduct;
                _context.Stocks.Add(stock);
                _context.Variants.Add(newVariant);


                for (int i = 0; i < product.Variants.Count; i++)
                {
                    dynamic option = product.Variants.ToArray()[i];
                    var newVariantAttriBute = new VariantAttribute()
                    {
                        AttributeTypeId = option.Id,
                        AttributeValue = variant.Variant[i],
                        Priority = i + 1
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
