﻿using arts_core.Data;
using arts_core.Models;
using Newtonsoft.Json;

namespace arts_core.Interfaces
{
    public interface ISeed
    {
        void SeedProductAndVariantData();
        void SeedUser();
        void SeedVariantAttribute();
    }

    public class Seed : ISeed
    {
        private DataContext _context;
        private IHostEnvironment _env;
        private ILogger<Seed> _logger;
        public Seed(DataContext dataContext, IHostEnvironment env, ILogger<Seed> logger)
        {
            _context = dataContext;
            _env = env;
            _logger = logger;
        }

        public void SeedProductAndVariantData()
        {
            try
            {
                var rootPath = _env.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "Data/product.json");

                var jsonData = System.IO.File.ReadAllText(fullPath);
                _logger.LogInformation(jsonData);
                if (string.IsNullOrWhiteSpace(jsonData))
                    _logger.LogError("Is Null Or WhiteSpace in Seed Product");

                var products = JsonConvert.DeserializeObject<List<Product>>(jsonData);
                if (products == null || products.Count == 0)
                    _logger.LogError("Product is Null To Seed");


                _context.Products.AddRange(products);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong in Seed");
            }
        }

        public void SeedUser()
        {
            try
            {
                var rootPath = _env.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "Data/user.json");
                var jsonData = System.IO.File.ReadAllText(fullPath);
                _logger.LogInformation(jsonData);
                if (string.IsNullOrWhiteSpace(jsonData))
                    _logger.LogError("Is Null Or WhiteSpace in Seed User");

                var users = JsonConvert.DeserializeObject<List<User>>(jsonData);
                if (users == null || users.Count == 0)
                    _logger.LogError("user is Null to Seed");
                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong in Seed");
            }
        }

        public void SeedVariantAttribute()
        {
            try
            {
                var rootPath = _env.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "Data/variantAttribute.json");
                var jsonData = System.IO.File.ReadAllText(fullPath);
                _logger.LogInformation(jsonData);
                if (string.IsNullOrWhiteSpace(jsonData))
                    _logger.LogError("Is Null Or WhiteSpace in Seed User");

                var variantAttribues = JsonConvert.DeserializeObject<List<VariantAttribute>>(jsonData);
                if (variantAttribues == null || variantAttribues.Count == 0)
                    _logger.LogError("user is Null to Seed");
                _context.VariantAttributes.AddRange(variantAttribues);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong in Seed");
            }
        }
    }
}
