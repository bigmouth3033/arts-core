using arts_core.Data;
using arts_core.Migrations;
using arts_core.Models;
using Faker;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;

namespace arts_core.Interfaces
{
    public interface ISeed
    {
        void SeedProductAndVariantData();

        void SeedVariantAttribute();
        void SeedAddress();
        void SeedPayments();
        Task SeedOrders();
        Task SeedUsers();
        Task SeedReview();
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
                var addressess = JsonConvert.DeserializeObject<List<Models.Address>>(jsonData);
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
        public async Task SeedOrders()
        {
            try
            {
                var random = new Random();
                for (int y = 0; y < 20; y++)
                {
                    int randomUserId = random.Next(120, 301);

                    var user = await _context.Users.Where(u => u.Id == randomUserId).SingleOrDefaultAsync();


                    for (int i = 0; i < 10; i++)
                    {

                        int randomVariant = random.Next(1, 29);

                        var variant = await _context.Variants.Where(v => v.Id == randomVariant).SingleOrDefaultAsync();
                        var quanityVariantOrder = 1;
                        var addressUser = await _context.Addresses.Where(u => u.UserId == user.Id).Take(1).SingleOrDefaultAsync();

                        var totalPrice = variant?.Price * quanityVariantOrder + 5;

                        if (variant?.Quanity > 0)
                        {
                            var payment = new Payment()
                            {
                                DeliveryTypeId = 1,
                                AddressId = addressUser.Id,
                                PaymentTypeId = 7,
                                ShipFee = 5,

                            };
                            var review = new Review()
                            {
                                Comment = "pauidfgjkbfi ayuildfgia7urg aeirughethw",
                                Rating = random.Next(1, 6),
                                UserId = randomUserId,
                                ProductId = variant.ProductId,
                                CreatedAt = DateTime.Now

                            };
                            var order = new Order()
                            {
                                
                                UserId = randomUserId,
                                Quanity = quanityVariantOrder,
                                OrderStatusId = 16,
                                TotalPrice = totalPrice,
                                Payment = payment,
                                VariantId = variant.Id,
                                Review = review,
                            };
                            variant.Quanity = variant.Quanity - quanityVariantOrder;
                            variant.AvailableQuanity = variant.AvailableQuanity - quanityVariantOrder;

                           
                            _context.Payments.Add(payment);
                            _context.Orders.Add(order);
                            _context.Reviews.Add(review);

                            _context.Variants.Update(variant);

                            await _context.SaveChangesAsync();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong in Seed");
            }
        }
        public async Task SeedReview()
        {

        }
        public async Task SeedUsers()
        {
            for (int i = 0; i < 100; i++)
            {
                var user = new User()
                {
                    Email = Faker.Name.First().ToLower() + Faker.Name.Suffix().ToLower() + "@gmail.com",
                    Fullname = Faker.Name.First(),
                    Password = "$2a$12$C1JQMjVl3bfXxtISNXv9Sulqsu/nEOx.yhtIozwvUW/DHWevhhQYG",
                    PhoneNumber = Faker.Phone.Number(),
                    RoleTypeId = 6,
                    Address = Faker.Address.StreetAddress(),
                    Verifired = true,

                };
                var address = new Models.Address()
                {
                    FullName = user.Fullname,
                    PhoneNumber = user.PhoneNumber,
                    AddressDetail = Faker.Address.StreetAddress(),
                    Province = "01",
                    District = "001",
                    Ward = "00001",
                    IsDefault = true,
                    User = user,
                };
                _context.Addresses.Add(address);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

        }
    }
}