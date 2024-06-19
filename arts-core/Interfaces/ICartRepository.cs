using arts_core.Data;
using arts_core.Models;
using Microsoft.EntityFrameworkCore;

namespace arts_core.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> CreateCartAsync(int userId, int variantId, int quanity);
        Task<Cart> FindCartByUserAndVariantAsync(int userId, int variantId);
        Task<List<Cart>> GetCartsByUserIdAsync(int userId);
        Task<UpdateCartModel> UpdateCartById(int cartId, int quanity);
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
                if (oldCart.UserId == userId && oldCart.VariantId == variantId)
                {
                    oldCart.Quanity += quanity;
                    Update(oldCart);
                    await _context.SaveChangesAsync();
                    return oldCart;
                }

                var cart = new Cart() { UserId = userId, VariantId = variantId, Quanity = quanity };
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
                    .Include(c => c.Variant).ThenInclude(v => v.VariantAttributes)
                    .Include(c => c.Variant).ThenInclude(v => v.Product).ThenInclude(p => p.ProductImages)
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
                _logger.LogError(ex, "Something wrong in when find Cart by UserId {userId} and VariantId {variantId}", userId, variantId);
            }
            return new Cart();
        }

        public async Task<UpdateCartModel> UpdateCartById(int cartId, int quanity)
        {
            string Name;
            float Price;
            int Quanity;
            float Total;
            try
            {
                var cart = await _context.Carts.FindAsync(cartId);
                var variant = await _context.Variants.Where(v => v.Id == cart.VariantId).Include(v => v.Product).FirstOrDefaultAsync();
                if (cart == null || variant == null)
                    return new UpdateCartModel(false, "Update Cart false because cart or variant is null");

                //kiem tra available stock cua quanity
                if (variant.AvailableQuanity >= quanity)
                {
                    cart.Quanity = quanity;
                    _context.Update(cart);
                    _context.SaveChanges();

                    Name = variant.Product.Name;
                    Price = variant.Price;                    
                    Quanity = cart.Quanity;
                    Total = Price * Quanity;
                    return new UpdateCartModel(true, $"Update CartId {cartId} Okay", Name, Price, Quanity, Total);
                }
                else
                {
                    cart.Quanity = variant.AvailableQuanity;
                    _context.Update(cart);
                    _context.SaveChanges();


                    Name = variant.Product.Name;
                    Price = variant.Price;                    
                    Quanity = variant.AvailableQuanity;
                    Total = Price * Quanity;
                    return new UpdateCartModel(false, $"Update CartId {cartId} fail", Name, Price, Quanity, Total);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "cannot update cart with cartId {cartId}", cartId);
            }
            return new UpdateCartModel(true, "Okay");
        }
    }

    public struct UpdateCartModel
    {
        public bool isOkay { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; }
        public int Quanity { get; set; }
        public float Total { get; set; }
        public UpdateCartModel(bool isokay, string message, string name, float price, int quanity, float total)
        {
            isOkay = isokay;
            Message = message;
            Name = name;
            Price = price;
            Quanity = quanity;
            Total = total;
        }
        public UpdateCartModel(bool isokay, string message)
        {
            isOkay = isokay;
            Message = message;
        }
    }
}
