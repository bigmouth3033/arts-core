using arts_core.Data;
using arts_core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace arts_core.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> CreateCartAsync(int userId, int variantId, int quanity);
        Task<Cart> FindCartByUserAndVariantAsync(int userId, int variantId);
        Task<List<Cart>> GetCartsByUserIdAsync(int userId);
    }


    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly ILogger<CartRepository> _logger;
        public CartRepository(ILogger<CartRepository> logger, DataContext dataContext) : base(dataContext)
        {
            _logger = logger;
        }

        public async Task<Cart> CreateCartAsync(int userId, int variantId, int quanity)
        {
            try
            {
                //find cart if created before
                var oldCart = await FindCartByUserAndVariantAsync(userId, variantId);
                if(oldCart.UserId == userId && oldCart.VariantId == variantId)
                {
                    oldCart.Quanity += quanity;
                    Update(oldCart);
                    await _context.SaveChangesAsync();
                    return oldCart;
                }

                var cart = new Cart() { UserId = userId, VariantId = variantId, Quanity = quanity};
                Add(cart);
                await _context.SaveChangesAsync();
                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot CreateCartAsync in CartRepository");
            }
            return null;
        }
        public async Task<List<Cart>> GetCartsByUserIdAsync(int userId)
        {
            try
            {
                var carts = await _context.Carts.Where(c => c.UserId == userId)
                    .Include(c => c.Variant).ThenInclude(v=> v.VariantAttributes)
                    .Include(c => c.Variant).ThenInclude(v => v.Product).ThenInclude(p =>p.ProductImages)
                    .ToListAsync();                
                return carts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong when get carts by UserId {userId}", userId);
            }        
            return new List<Cart>();
        }
        public async Task<Cart> FindCartByUserAndVariantAsync(int userId, int variantId)
        {
            try
            {
                var cart = await _context.Carts.Where(c => c.UserId == userId && c.VariantId == variantId).FirstOrDefaultAsync();
                if (cart == null)
                    return new Cart();
                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong in when find Cart by UserId {userId} and VariantId {variantId}",userId,variantId);
            }
            return new Cart();
        }
    }

    public class VariantResponse
    {

    }

    public class UserResponse
    {

    }
    public class ProductResponse
    {

    }

    public class ProductImageResponse
    {

    }
}
