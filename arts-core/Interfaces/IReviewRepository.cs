using arts_core.Data;
using arts_core.Models;

namespace arts_core.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<CustomResult>> GetAllReviewProductAsync();
        Task<IEnumerable<CustomResult>> GetAllReviewProductByUserAsync();
        CustomResult CreateReview(int userId, RequestModels.RequestReview requestRequest);

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

        public CustomResult CreateReview(int userId, RequestModels.RequestReview requestRequest)
        {
            try
            {
                var review = new Review()
                {
                    Comment = requestRequest.Comment,
                    ProductId = requestRequest.ProductId,
                    Rating = requestRequest.Rating,
                    UserId = userId,
                };

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



        public Task<IEnumerable<CustomResult>> GetAllReviewProductAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CustomResult>> GetAllReviewProductByUserAsync()
        {
            throw new NotImplementedException();
        }
    }

}
