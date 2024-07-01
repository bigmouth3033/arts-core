using arts_core.Data;
using arts_core.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace arts_core.Interfaces
{
    public interface IExchangeRepository : IRepository<Exchange>
    {
        Task<CustomResult> CreateExchangeAsync(ExchangeRequest request);
        Task<CustomResult> UpdateExchangeAsync(UpdateExchangeRequest request);
    }
    public class ExchangeRepository : GenericRepository<Exchange>,IExchangeRepository
    {
        private readonly ILogger<ExchangeRepository> _logger;
        public ExchangeRepository(ILogger<ExchangeRepository> logger,DataContext dataContext):base(dataContext)
        {
            _logger  = logger;
        }

        public async Task<CustomResult> CreateExchangeAsync(ExchangeRequest request)
        {
            try
            {
                //kiem tra order da tung refund hoac exchange chua
                var order = await _context.Orders
                    .Include(od => od.Refund)
                    .Include(od =>od.Exchanges)
                    .FirstOrDefaultAsync(od => od.Id == request.OriginalOrderId);

                if(order.Exchanges.Count > 0  || order.Refund != null)                
                    return new CustomResult(400, "Order has been exchanged or refund before", null);
                


                var exchange = new Exchange() { OriginalOrderId = request.OriginalOrderId,ReasonExchange = request.ReasonExchange};
                  _context.Exchanges.Add(exchange);
                await _context.SaveChangesAsync();
                return new CustomResult(200, "Exchange has been delivered",null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in CreateExchangeAsync ");
                throw;
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

                if(request.Status == "Denied")
                {
                    exchange.ResponseExchange = request.ResponseExchange;
                    exchange.Status = request.Status;
                    _context.Exchanges.Update(exchange);
                    var reulst = await _context.SaveChangesAsync();
                    transaction.Commit();
                    return new CustomResult(201, "Update Exchange denid Successfully", null);
                }

                //tao order bang order cu
                var oldOrderExchange  = await _context.Orders.FirstOrDefaultAsync(o => o.Id == exchange.OriginalOrderId);
                var newOrderExchange = new Order()
                {
                    Id= 0,
                    UserId = oldOrderExchange.UserId,
                    VariantId = oldOrderExchange.VariantId,
                    Quanity = oldOrderExchange.Quanity,
                    OrderStatusId = oldOrderExchange.OrderStatusId,
                    TotalPrice = oldOrderExchange.TotalPrice,
                    CreatedAt = DateTime.Now,
                    PaymentId = oldOrderExchange.PaymentId
                };
               

                exchange.NewOrder = newOrderExchange;
                exchange.ResponseExchange = request.ResponseExchange;
                exchange.Status = request.Status;

                var variant = await _context.Variants.FirstOrDefaultAsync(v => v.Id == oldOrderExchange.VariantId);
                variant.Quanity -= newOrderExchange.Quanity;
                variant.AvailableQuanity -= newOrderExchange.Quanity;

                if (variant.AvailableQuanity < 0 || variant.AvailableQuanity < 0)
                    return new CustomResult(401, $"Quanity of variant {variant.Id} lower than 0 please update variant before Exchange", null);

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
    }
    public class UpdateExchangeRequest
    {
        public int ExchangeId { get; set; }
        public string ResponseExchange { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

}
