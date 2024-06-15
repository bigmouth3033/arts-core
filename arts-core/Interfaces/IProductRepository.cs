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

        public Task<CustomPaging> GetPagingProducts(int pageNumber, int pageSize);

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
            if(product.VariantDetailsJSON != null)
            {
                foreach (var json in product.VariantDetailsJSON)
                {
                    var detail = JsonConvert.DeserializeObject<VariantDetail>(json);
                    product.VariantDetails.Add(detail);
                }

                foreach (var json in product.VariantsJSON)
                {
                    var variant = JsonConvert.DeserializeObject<RequestModels.Variant>(json);
                    product.Variants.Add(variant);
                }
            }
          

            var newProduct = new Product()
            {
                CategoryId = product.Category,
                Name = product.ProductName,
                Description = product.Description,
                IsActive = product.Active,
                WarrantyDuration = product.Warranty
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

            if(product.VariantDetails.Count == 0)
            {
                var newVariant = new Models.Variant()
                {
                    Active = true,
                    Price = product.Price,
                    SalePrice = product.SalePrice,
                    Quanity = 0
                };

                newVariant.Product = newProduct;
                _context.Variants.Add(newVariant);
            }

            foreach (dynamic variant in product.VariantDetails)
            {
                var newVariant = new Models.Variant() {
                    Active = true,
                    VariantImage = variant.Image != null ? imageNameList.ToArray()[variant.Image] : null,
                    Price = variant.SellPrice,
                    SalePrice = variant.ComparePrice,
                    Quanity = variant.Inventory,
                };

                if(variant.Inventory != 0)
                {
                    var stock = new Stock
                    {
                        CostPerItem = variant.BeginFund,
                        Quantity = variant.Inventory,
                        Variant = newVariant
                    };

                    _context.Stocks.Add(stock);
                }

                newVariant.Product = newProduct;
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

        public async Task<CustomPaging> GetPagingProducts(int pageNumber, int pageSize)
        {
            var list = await _context.Products.Include(p => p.Category).Include(p => p.ProductImages).Include(p => p.Variants).ThenInclude(p => p.VariantAttributes).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(); 

            var total = _context.Products.Count();

            var customPaging = new CustomPaging()
            {
                Status = 200,
                Message = "OK",
                CurrentPage = pageNumber,
                TotalPages = (total / pageSize),
                PageSize = pageSize,
                TotalCount = total,
                Data = list
            };

            return customPaging;
        }
    }
}
