﻿using arts_core.Data;
using arts_core.Models;
using arts_core.RequestModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Buffers;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc;
using System;

namespace arts_core.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        public CustomResult CreateProduct(CreateProduct product);

        public Task<CustomPaging> GetPagingProducts(int pageNumber, int pageSize, IEnumerable<int> categoryId, string searchValue);

        public Task<CustomResult> GetProduct(int id);

        public Task<CustomResult> GetProductAdmin(int id);

        public Task<CustomResult> GetProductVariantInfo(int id);

        public Task<CustomResult> CreateImages(RequestImages images);

        public Task<CustomResult> DeleteImage(int imageId);

        //Paginagion for product listing-page
        public Task<CustomPaging> GetPagingProductForListingPage(int categoryId, int pageNumber, int pageSize, int sort, string searchValue, float priceRangeMin, float priceRangeMax);
        public Task<CustomResult> UpdateProduct(UpdateProduct product);

        public Task<CustomResult> SearchProduct(string searchValue);

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
                WarrantyDuration = product.Warranty,
                Unit = product.Unit
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
                    Quanity = product.Amount,
                    AvailableQuanity = product.Amount
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
                    AvailableQuanity = variant.Inventory,
                };


                newVariant.Product = newProduct;
                _context.Variants.Add(newVariant);


                for (int i = 0; i < product.Variants.Count; i++)
                {
                    dynamic option = product.Variants.ToArray()[i];
                    var newVariantAttriBute = new VariantAttribute()
                    {
                        AttributeTypeId = option.Id,
                        AttributeValue = variant.Variant[i].Trim(),
                        Priority = i + 1
                    };
                    newVariantAttriBute.Variant = newVariant;
                    _context.VariantAttributes.Add(newVariantAttriBute);
                }
            }

            _context.Products.Add(newProduct);

            return new CustomResult(200, "success", newProduct);
        }

        public async Task<CustomPaging> GetPagingProducts(int pageNumber, int pageSize, IEnumerable<int> categoryId, string searchValue)
        {
            IQueryable<Product> query;

            query = _context.Products;

            if(categoryId.Count() != 0)
            {
                query = query.Where(p => categoryId.Contains(p.CategoryId));
            }

            query = query.Where(p => p.Name.Contains(searchValue));

            query = query.OrderByDescending(p => p.CreatedAt);

     
            query = query.Include(p => p.Category)
                         .Include(p => p.ProductImages)
                         .Include(p => p.Variants)
                            .ThenInclude(v => v.VariantAttributes);

            var total = query.Count();

            query = query.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize);

      
            var list = await query.ToListAsync();

            var customPaging = new CustomPaging()
            {
                Status = 200,
                Message = "OK",
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)total / pageSize),
                PageSize = pageSize,
                TotalCount = total,
                Data = list
            };

            return customPaging;


        }

        public async Task<CustomResult> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products.Include(p => p.ProductImages).Include(p => p.Category).Include(p => p.Variants).ThenInclude(p => p.VariantAttributes).ThenInclude(p => p.AttributeType).SingleOrDefaultAsync(p => p.Id == id && p.IsActive == true);

                if(product == null)
                {
                    return new CustomResult(404, "failed", null); 
                }

                return new CustomResult(200, "success", product);
            }catch (Exception ex)
            {
                return new CustomResult(400, "failed", ex.Message);
            }
        }

        public async Task<CustomResult> GetProductVariantInfo(int id)
        {
            var variants = await _context.Variants.Where(v => v.ProductId == id).Select(v => v.Id).ToListAsync();

            var variantAttributes = await _context.VariantAttributes.Include(v => v.AttributeType).Where(v => variants.Contains(v.VariantId)).GroupBy(va => new { va.Priority, va.AttributeType.Name })
            .Select(g => new
            {
                Priority = g.Key.Priority,
                Variant = g.Key.Name,
                Values =  RemoveDuplicates(g.Select(va => va.AttributeValue).ToList())
            })
            .ToListAsync();

            return new CustomResult(200, "success", variantAttributes);
        }

        static List<string> RemoveDuplicates(List<string> array)
        {
            var seen = new HashSet<string>();
            var result = new List<string>();

            foreach (var item in array)
            {
                if (seen.Add(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public async Task<CustomResult> CreateImages(RequestImages images)
        {
            try
            {
                var listImages = new List<string>();

                var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == images.ProductId);

                foreach (var item in images.Images)
                {
                    var fileName = DateTime.Now.Ticks + item.FileName;
                    listImages.Add(fileName);
                    var uploadPath = Path.Combine(_env.WebRootPath, "images");
                    var filePath = Path.Combine(uploadPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    item.CopyTo(stream);

                    var newImage = new ProductImage
                    {
                        ImageName = fileName,
                        Product = product
                    };

                    _context.ProductImages.Add(newImage);
                }

                return new CustomResult(200, "success", listImages);
            }
            catch(Exception ex)
            {
                return new CustomResult(400, "Bad Request", ex.Message);
            }
        }

        public async Task<CustomResult> DeleteImage(int imageId)
        {
            try
            {
                var image = await _context.ProductImages.SingleOrDefaultAsync(img => img.Id == imageId);

                _context.ProductImages.Remove(image);

                return new CustomResult(200, "success", null);
            }
            catch(Exception ex)
            {
                return new CustomResult(400, "Bad Request", ex.Message);
            }
        }

        public async Task<CustomResult> GetProductAdmin(int id)
        {
            try
            {
                var product = await _context.Products.Include(p => p.ProductImages).Include(p => p.Category).Include(p => p.Variants).ThenInclude(p => p.VariantAttributes).ThenInclude(p => p.AttributeType).SingleOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return new CustomResult(404, "failed", null);
                }

                return new CustomResult(200, "success", product);
            }
            catch (Exception ex)
            {
                return new CustomResult(400, "failed", ex.Message);
            }
        }

        public async Task<CustomPaging> GetPagingProductForListingPage(int categoryId, int pageNumber, int pageSize, int sort, string searchValue, float priceRangeMin, float priceRangeMax)
        {
            /*var list = await _context.Products.Include(p => p.Category).Include(p => p.ProductImages).Include(p => p.Variants).ThenInclude(p => p.VariantAttributes).Where(p => p.CategoryId == categoryId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();*/



            // Step 1: Filter by Category
            IQueryable<Product> query;

            if (categoryId == 0)
            {
                query = _context.Products;
            }
            else
            {
                query = _context.Products
                                .Where(p => p.CategoryId == categoryId);
            }


            //Step 2: Search
            query = query.Where(p => p.Name.Contains(searchValue));

            // Step 3: Include Related Entities
            query = query.Include(p => p.ProductImages)
                         .Include(p => p.Variants);

            // Step 4: Filter by price range


            query = query.Where(p => p.Variants.Any(v => v.Price >= priceRangeMin && v.Price <= priceRangeMax));


            // Step 5: Sort
            if (sort == 1)
            {
                query = query.OrderByDescending(p => p.CreatedAt);
            }
            //sort Low to High price(Min Price (base on Variant) of A Product)
            if (sort == 2)
            {
                query = query.OrderBy(p => p.Variants.Min(v => v.Price));
            }

            //sort High to Low price
            if (sort == 3)
            {
                query = query.OrderByDescending(p => p.Variants.Min(v => v.Price));
            }

            // Step 6: Apply Pagination
            var total = query.Count();
            query = query.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize);

            // Materialize the query results before using them
            var list = await query.ToListAsync();

            var customPaging = new CustomPaging()
            {
                Status = 200,
                Message = "OK",
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)total / pageSize),
                PageSize = pageSize,
                TotalCount = total,
                Data = list
            };

            return customPaging;

        }

        public async Task<CustomResult> UpdateProduct(UpdateProduct product)
        {

            try
            {
                if (product.VariantDetailsJSON != null)
                {
                    foreach (var json in product.VariantDetailsJSON)
                    {
                        var detail = JsonConvert.DeserializeObject<VariantUpdate>(json);
                        product.VariantDetails.Add(detail);
                    }

                    foreach (var json in product.VariantsJSON)
                    {
                        var variant = JsonConvert.DeserializeObject<RequestModels.Variant>(json);
                        product.Variants.Add(variant);
                    }
                }

                var oldProduct = await _context.Products.SingleOrDefaultAsync(p => p.Id == product.ProductId);

                oldProduct.CategoryId = product.Category;
                oldProduct.Name = product.ProductName;
                oldProduct.Description = product.Description;
                oldProduct.IsActive = product.Active;
                oldProduct.WarrantyDuration = product.Warranty;
                oldProduct.Unit = product.Unit;

                if (product.VariantDetails.Count == 0)
                {
                    var variant = await _context.Variants.SingleOrDefaultAsync(v => v.ProductId == product.ProductId);

                    variant.Active = true;
                    variant.Price = product.Price;
                    variant.SalePrice = product.SalePrice;
                    variant.Quanity = (variant.AvailableQuanity - variant.Quanity) + product.Amount;
                    variant.AvailableQuanity = product.Amount;
                    _context.Variants.Update(variant);
                }

                foreach (VariantUpdate variant in product.VariantDetails)
                {
                    var oldvariant = await _context.Variants.SingleOrDefaultAsync(v => v.Id == variant.Id);

                    oldvariant.VariantImage = variant.VariantImage;
                    oldvariant.Price = variant.Price;
                    oldvariant.SalePrice = variant.SalePrice;
                    oldvariant.Quanity = (oldvariant.Quanity - oldvariant.AvailableQuanity) + variant.AvailableQuanity;
                    oldvariant.AvailableQuanity = variant.AvailableQuanity;


                    _context.Variants.Update(oldvariant);
                }



                return new CustomResult(200, "success", null);
            }
            catch(Exception ex)
            {
                return new CustomResult(400, "Failed", ex.Message);
            }
          
        }

        public async Task<CustomResult> SearchProduct(string searchValue)
        {
            try
            {
                var products = await _context.Products.Include(p => p.ProductImages).Where(p => p.Name.Contains(searchValue)).ToListAsync();
                
                return new CustomResult(200, "Success", products);
            }catch(Exception ex)
            {
                return new CustomResult(400, "Failed", ex.Message);
            }
        }
    }
}
