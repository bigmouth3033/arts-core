using arts_core.Data;
using arts_core.Models;
using arts_core.Service;
using Microsoft.EntityFrameworkCore;

namespace arts_core.Interfaces
{
    public interface IExchangeRepository : IRepository<Exchange>
    {
        Task<CustomResult> CreateExchangeAsync(ExchangeRequest request);
        Task<CustomResult> UpdateExchangeAsync(UpdateExchangeRequest request);

        Task<CustomResult> GetExchangeById(int exchangeId);
    }

    public class ExchangeRepository : GenericRepository<Exchange>, IExchangeRepository
    {
        private readonly ILogger<ExchangeRepository> _logger;
        private readonly IFileService _fileService;
        public ExchangeRepository(ILogger<ExchangeRepository> logger, DataContext dataContext, IFileService fileService) : base(dataContext)
        {
            _logger = logger;
            _fileService = fileService;
        }

        public async Task<CustomResult> CreateExchangeAsync(ExchangeRequest request)
        {
            try
            {
                //kiem tra order da tung refund hoac exchange chua
                var order = await _context.Orders
                    .Include(od => od.Refund)
                    .Include(od => od.Exchange)
                    .FirstOrDefaultAsync(od => od.Id == request.OriginalOrderId);

                if ( order.Refund != null)
                    return new CustomResult(400, "Order has been exchanged or refund before", null);

                var images = new List<StoreImage>();
                if (request.Images != null)
                {
                    var imageRoots = await _fileService.StoreImageAsync("Images", request.Images);
                    foreach (var imageRoot in imageRoots)
                    {
                        var imageName = imageRoot;
                        string entityName = "Refunds";
                        var storeImage = new StoreImage()
                        {
                            EntityName = entityName,
                            ImageName = imageName
                        };
                        images.Add(storeImage);
                    }
                    _logger.LogInformation($"filename: {imageRoots}");
                }

                order.UpdatedAt = DateTime.Now;

                _context.Orders.Update(order);

                var exchange = new Exchange() { 
                    OriginalOrderId = request.OriginalOrderId,
                    ReasonExchange = request.ReasonExchange,
                    Images = images
                };
                _context.Exchanges.Add(exchange);
                await _context.SaveChangesAsync();
                return new CustomResult(200, "Exchange has been delivered", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in CreateExchangeAsync ");
                throw;
            }
        }

        public async Task<CustomResult> GetExchangeById(int exchangeId)
        {
            try
            {
                var exchange = await _context.Exchanges.Include(e => e.Images).SingleOrDefaultAsync(e => e.Id == exchangeId);

                return new CustomResult(200, "Success", exchange);
            }catch(Exception ex)
            {
                return new CustomResult(400, "Failed", ex.Message);
            }
        }

        public async Task<CustomResult> UpdateExchangeAsync(UpdateExchangeRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var exchange = await _context.Exchanges.FirstOrDefaultAsync(e => e.Id == request.ExchangeId);

                if (exchange.Status == "Success" || exchange.Status == "Denied")
                    return new CustomResult(402, "Cannot Update Exchange again", null);

                if (request.Status == "Denied")
                {
                    exchange.ResponseExchange = request.ResponseExchange;
                    exchange.Status = request.Status;
                    _context.Exchanges.Update(exchange);
                    var reulst = await _context.SaveChangesAsync();
                    transaction.Commit();
                    return new CustomResult(201, "Update Exchange denid Successfully", null);
                }

                //tao order bang order cu
                var oldOrderExchange = await _context.Orders.FirstOrDefaultAsync(o => o.Id == exchange.OriginalOrderId);
                var newOrderExchange = new Order()
                {
                    Id = 0,
                    UserId = oldOrderExchange.UserId,
                    VariantId = oldOrderExchange.VariantId,
                    Quanity = oldOrderExchange.Quanity,
                    OrderStatusId = 17,
                    TotalPrice = oldOrderExchange.TotalPrice,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    PaymentId = oldOrderExchange.PaymentId
                };


                exchange.NewOrder = newOrderExchange;
                exchange.ResponseExchange = request.ResponseExchange;
                exchange.Status = request.Status;

                var variant = await _context.Variants.FirstOrDefaultAsync(v => v.Id == oldOrderExchange.VariantId);
                variant.Quanity -= newOrderExchange.Quanity;
                variant.AvailableQuanity -= newOrderExchange.Quanity;

                if (variant.AvailableQuanity < 0 || variant.AvailableQuanity < 0)
                    return new CustomResult(401, $"Quanity of this product lower than 0 please update variant before Exchange", null);

                _context.Variants.Update(variant);
                _context.Exchanges.Update(exchange);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Something wrong in UpdateExchangeAsync");
                throw;
            }
            return new CustomResult(200, "gaga", null);
        }
    }
    public class ExchangeRequest
    {
        public int OriginalOrderId { get; set; }
        public string ReasonExchange { get; set; } = string.Empty;
        public ICollection<IFormFile>? Images { get; set; }
    }
    public class UpdateExchangeRequest
    {
        public int ExchangeId { get; set; }
        public string? ResponseExchange { get; set; }
        public string? Status { get; set; }
    }
}
