using arts_core.Data;
using arts_core.Models;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace arts_core.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<CustomPaging> GetAllOrderAdmin(int pageNumber, int pageSize);
    }
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ILogger<OrderRepository> _logger;
        public OrderRepository(ILogger<OrderRepository> logger, DataContext dataContext) : base(dataContext)
        {
            _logger = logger;
        }

        public async Task<CustomPaging> GetAllOrderAdmin(int pageNumber, int pageSize)
        {
            try
            {
                var orders = await _context.Orders.Include(o => o.User).Include(o => o.Payment).ThenInclude(p => p.PaymentType ).Include(o => o.Payment).ThenInclude(o => o.DeliveryType).Include(o => o.Payment).ThenInclude(o => o.Address).Include(o => o.Variant).ThenInclude(o => o.Product).ToListAsync();

                var total = _context.Orders.Count();

                var customPaging = new CustomPaging()   
                {
                    Status = 200,
                    Message = "OK",
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling((double)total / pageSize),
                    PageSize = pageSize,
                    TotalCount = total,
                    Data = orders
                };


                return customPaging;

            }
            catch (Exception ex)
            {
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
    }
}
