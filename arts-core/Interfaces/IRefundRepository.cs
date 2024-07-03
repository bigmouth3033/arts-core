﻿using arts_core.Data;
using arts_core.Models;
using arts_core.Service;
using Microsoft.EntityFrameworkCore;

namespace arts_core.Interfaces
{
    public interface IRefundRepository : IRepository<Refund>
    {
        Task<CustomResult> CreateRefundAsync(RefundRequest request);
        Task<CustomResult> GetRefundsByUserIdAsync(int userId);
        Task<CustomResult> GetAllRefundsAsync();
        Task<CustomResult> UpdateRefundAsync(RefundReQuestForAdmin request);
    }
    public class RefundRepository : GenericRepository<Refund>, IRefundRepository
    {
        private readonly ILogger<RefundRepository> _logger;
        private readonly IFileService _fileService;
        public RefundRepository(ILogger<RefundRepository> logger, IFileService fileService, DataContext dataContext) : base(dataContext)
        {
            _logger = logger;
            _fileService = fileService;
        }
        public async Task<CustomResult> CreateRefundAsync(RefundRequest request)
        {
            bool isExpired = false;
            try
            {
                //check refund expired 
                var order = await _context.Orders.Include(od => od.Variant).FirstOrDefaultAsync(o => o.Id == request.OrderId);
                isExpired = isOrderOlderThan7Days(order);
                if (isExpired)
                    return new CustomResult(401, "Order must be within 7 days to Refund", null);

                //kiem tra co order nao da tung refund khong
                var refunds = await _context.Refunds.Where(r => r.OrderId == request.OrderId).FirstOrDefaultAsync();
                if (refunds != null)
                    return new CustomResult(402, "Order had been refund before", null);

                //them hin anh neu co
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


                var refund = new Refund()
                {
                    OrderId = order.Id,
                    ReasonRefund = request.ReasonRefund,
                    AmountRefund = (float)order.TotalPrice,
                    Images = images
                };


                _context.Refunds.Add(refund);
                await _context.SaveChangesAsync();
                return new CustomResult(200, "Your refund has been delivery success", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "something wrong in CreateRefund");
                throw;
            }
        }

        public async Task<CustomResult> GetRefundsByUserIdAsync(int userId)
        {
            try
            {
                var refunds = await _context.Refunds
                    .Include(r => r.Order)
                    .Include(r => r.Images)
                    .OrderByDescending(r => r.CreatedAt)
                    .Where(r => r.Order.UserId == userId)
                    .ToListAsync();

                if (refunds.Count == 0)
                    return new CustomResult(400, "Not Found", refunds);

                return new CustomResult(200, "Get successfull", refunds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "something wrong in GetRefundsByUserId");
                throw;
            }
        }
        public async Task<CustomResult> GetAllRefundsAsync()
        {
            try
            {
                var refunds = await _context.Refunds
                   .Include(r => r.Order.Variant)
                   .Include(r => r.Order.User)
                   .OrderByDescending(r => r.CreatedAt).ToListAsync();
                return new CustomResult(200, "Get All Refunds", refunds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "something wrong in GetAllRefundsAsync");
                throw;
            }
        }
        public async Task<CustomResult> UpdateRefundAsync(RefundReQuestForAdmin request)
        {
            try
            {
                var refund = await _context.Refunds.FindAsync(request.RefundId);
                if (refund == null)
                    return new CustomResult(400, "Refund Not Found", null);

                if (refund.Status == "Success" || refund.Status == "Denied")
                    return new CustomResult(401, "Refund Can't Update when success or denied", null);

                refund.ResponseRefund = request.ResponseRefund;
                refund.Status = request.Status;
                refund.UpdatedAt = DateTime.Now;
                _context.Refunds.Update(refund);
                await _context.SaveChangesAsync();
                return new CustomResult(200, "Update refund is Sucess", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "something went wrong in UpdateRefundAsync");
                throw;
            }

        }

        private bool isOrderOlderThan7Days(Order order)
        {
            return (DateTime.Now - order.CreatedAt).TotalDays > 7;
        }

    }
    public class RefundRequest
    {
        public int OrderId { get; set; }
        public string ReasonRefund { get; set; } = string.Empty;
        public ICollection<IFormFile>? Images { get; set; }     
    }
    public class RefundReQuestForAdmin
    {
        public int RefundId { get; set; }
        public string ResponseRefund { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public RefundReQuestForAdmin(int refundId, string responseRefund, string status)
        {
            RefundId = refundId;
            ResponseRefund = responseRefund;
            Status = status;
        }
    }
}
