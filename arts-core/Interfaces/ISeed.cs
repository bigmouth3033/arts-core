using arts_core.Data;
using arts_core.Models;
using Newtonsoft.Json;

namespace arts_core.Interfaces
{
    public interface ISeed
    {
        void SeedProductAndVariantData();
        void SeedUser();
        void SeedVariantAttribute();
        void SeedAddress();
        void SeedPayments();
        void SeedOrders();
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
                var fullPath = Path.Combine(rootPath, "Data/variantAttributes.json");
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
        public void SeedAddress()
        {
            try
            {
                var rootPath = _env.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "Data/address.json");
                var jsonData = System.IO.File.ReadAllText(fullPath);
                _logger.LogInformation(jsonData);
                if (string.IsNullOrWhiteSpace(jsonData))
                    _logger.LogError("Is Null Or WhiteSpace in Seed Address");
                var addressess = JsonConvert.DeserializeObject<List<Address>>(jsonData);
                if (addressess == null || addressess.Count == 0)
                    _logger.LogError("address is Null to Seed");
                _context.Addresses.AddRange(addressess);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong in Seed");
            }
        }
        public void SeedPayments()
        {
            try
            {
                var rootPath = _env.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "Data/payments.json");
                var jsonData = System.IO.File.ReadAllText(fullPath);
                _logger.LogInformation(jsonData);
                if (string.IsNullOrWhiteSpace(jsonData))
                    _logger.LogError("Is Null Or WhiteSpace in Seed User");

                var payments = JsonConvert.DeserializeObject<List<Payment>>(jsonData);
                if (payments == null || payments.Count == 0)
                    _logger.LogError("payments is Null to Seed");
                _context.Payments.AddRange(payments);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong in Seed");
            }
        }
        public void SeedOrders()
        {
            try
            {
                var rootPath = _env.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "Data/orders.json");
                var jsonData = System.IO.File.ReadAllText(fullPath);
                _logger.LogInformation(jsonData);
                if (string.IsNullOrWhiteSpace(jsonData))
                    _logger.LogError("Is Null Or WhiteSpace in Seed User");

                var orders = JsonConvert.DeserializeObject<List<Order>>(jsonData);
                if (orders == null || orders.Count == 0)
                    _logger.LogError("orders is Null to Seed");
                _context.Orders.AddRange(orders);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong in Seed");
            }
        }
    }
}
