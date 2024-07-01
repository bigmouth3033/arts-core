using arts_core.Data;
using arts_core.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace arts_core.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<CustomPaging> GetAllReviewProductAsync(int productId, int pageNumber, int pageSize, int star);
        Task<CustomResult> GetAllReviewProductByUserAsync(int userId);
        Task<CustomResult> CreateReview(int userId, RequestModels.RequestReview requestRequest);
        Task<CustomResult> CheckReview(int userId, int productId);
        Task<CustomResult> TotalStar(int productId);

    }
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly ILogger<ReviewRepository> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        public ReviewRepository(DataContext dataContext, ILogger<ReviewRepository> logger, IConfiguration configuration, IWebHostEnvironment env) : base(dataContext)
        {
            _logger = logger;
            _config = configuration;
            _env = env;
        }

        public async Task<CustomResult> CheckReview(int userId, int productId)
        {
            try
            {
                var order = await _context.Orders.Include(v => v.Variant).ThenInclude(p => p.Product).Where(o => o.UserId == userId && o.Variant.Product.Id == productId && o.OrderStatusId == 16 && o.ReviewId == null).Select((o)=> o.Id).ToListAsync();
                return new CustomResult(200, "success", order);
            }catch (Exception ex)
            {
                return new CustomResult(400, "fail", null);

            }
        }

        public async Task<CustomResult> CreateReview(int userId, RequestModels.RequestReview requestRequest)
        {
            try

            {


                var review = new Review()
                {
                    Comment = requestRequest.Comment,
                    ProductId = requestRequest.ProductId,
                    Rating = requestRequest.Rating,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                };
                var order = await _context.Orders.SingleOrDefaultAsync(o => o.Id == requestRequest.OrderId);

                order.Review = review;
                _context.Orders.Update(order);

                if (requestRequest.Images != null)
                {
                    foreach (var image in requestRequest.Images)
                    {
                        var fileName = DateTime.Now.Ticks + image.FileName;
                        var uploadPath = Path.Combine(_env.WebRootPath, "images");
                        var filePath = Path.Combine(uploadPath, fileName);

                        using var stream = new FileStream(filePath, FileMode.Create);
                        image.CopyTo(stream);

                        var newImage = new ReviewImage
                        {
                            ImageName = fileName,
                            Review = review,
                        };

                        _context.ReviewImages.Add(newImage);
                    }
                }
                _context.Reviews.Add(review);
                return new CustomResult(200, "Success", review);
            }
            catch (Exception ex)
            {
                return new CustomResult(400, "Error", ex);
            }
        }



        public async Task<CustomPaging> GetAllReviewProductAsync(int productId, int pageNumber, int pageSize, int star)
        {
            try
            {
                var reviews = _context.Reviews.Include(o => o.Order).ThenInclude(o => o.Variant).Include(o => o.User).Include(o => o.ReviewImages).Where(r => r.Order.Variant.ProductId == productId).OrderByDescending(r => r.CreatedAt).AsNoTracking();

                if (star != 0)
                {
                    reviews = reviews.Where(r => r.Rating == star);
                }

                var total = reviews.Count();

                reviews = reviews.Skip((pageNumber - 1) * pageSize)
                     .Take(pageSize);
                var list = await reviews.ToListAsync();

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
            catch (Exception ex) {
                return new CustomPaging()
                {
                    Status = 400,
                    Message = "Failed",
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling((double)0 / pageSize),
                    PageSize = pageSize,
                    TotalCount = 0,
                    Data = null
                };

            }
        }

        public async Task<CustomResult> GetAllReviewProductByUserAsync(int userId)
        {

            try
            {
                var reviews = await _context.Reviews.Include(o => o.Order).ThenInclude(o => o.Variant).Include(p=>p.Product).ThenInclude(p=>p.ProductImages).Include(o => o.ReviewImages).Where(u=>u.UserId ==userId).OrderByDescending(r => r.CreatedAt).AsNoTracking().ToListAsync();
                return new CustomResult(200, "success", reviews);
            }
            catch (Exception ex)
            {
                return new CustomResult(400, "fail", null);
            }
        }

        public async Task<CustomResult> TotalStar(int productId)
        {
            try
            {
                var totalStar = await _context.Reviews.Include(o => o.Order).ThenInclude(o => o.Variant).Where(r => r.Order.Variant.ProductId == productId).GroupBy(o=>o.Rating).Select(o=>new{
                    star = o.Key,
                    amount = o.Count(),
                }).ToListAsync();
                return new CustomResult(200, "success", totalStar);
            }
            catch (Exception ex) {
                return new CustomResult(400, "fail", null);
            }
        }
    }

}
