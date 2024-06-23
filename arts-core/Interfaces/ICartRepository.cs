using arts_core.Data;
using arts_core.Models;
using Microsoft.EntityFrameworkCore;

namespace arts_core.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<UpdateCartModel> CreateCartAsync(string email, int variantId, int quanity);
        Task<Cart> FindCartByUserAndVariantAsync(int userId, int variantId);
        Task<List<Cart>> GetCartsByUserIdAsync(int userId);
        Task<UpdateCartModel> UpdateCartById(int cartId, int quanity);
        Task<UpdateCartModel> DeleteCartById(int cartId);
        Task<float> GetTotalAmountByCartsId(int[] idCarts);

        Task<CustomResult> GetUserCartQuantity(string email);
    }


    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly ILogger<CartRepository> _logger;
        public CartRepository(ILogger<CartRepository> logger, DataContext dataContext) : base(dataContext)
        {
            _logger = logger;
        }

        public async Task<UpdateCartModel> CreateCartAsync(string email, int variantId, int quanity)
        {
            string Name;
            float Price;
            int Quanity;
            float Total;
            //kiem tra xem card da tung tao chua va update

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            var olcCart = await _context.Carts.Where(c => c.UserId == user.Id && c.VariantId == variantId).SingleOrDefaultAsync();
            var variant = await _context.Variants.Include(v => v.Product).FirstOrDefaultAsync(v => v.Id == variantId);

            if (olcCart != null)
            {
                olcCart.Quanity += quanity;
                if (!CompareQuanityAndAvailableQuanity(olcCart.Quanity, variant.AvailableQuanity))
                {
                    Name = variant.Product.Name;
                    Price = variant.Price;
                    Quanity = variant.AvailableQuanity;
                    Total = Price * Quanity;
                    return new UpdateCartModel(false, "Update cart exist fail", Name, Price, Quanity, Total);
                }
                _context.Carts.Update(olcCart);
                await _context.SaveChangesAsync();

                return new UpdateCartModel(true, "Update cart success");
            }

            var cart = new Cart();
            cart.UserId = user.Id;
            cart.VariantId = variantId;
            cart.Quanity = quanity;

            if (!CompareQuanityAndAvailableQuanity(quanity, variant.AvailableQuanity))
            {
                return new UpdateCartModel(false, "Cannot create cart because quanity over than available quanity","",0, quanity: variant.AvailableQuanity,0);
            }
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return new UpdateCartModel(true, "Create cart success");
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

        public async Task<UpdateCartModel> DeleteCartById(int cartId)
        {
            try
            {
                var cart = _context.Carts.Find(cartId);
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
                return new UpdateCartModel(true, $"Delete Cart {cartId} suceessfull");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private bool CompareQuanityAndAvailableQuanity(int quanity, int availableQuanity)
        {
            if (quanity > availableQuanity)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<float> GetTotalAmountByCartsId(int[] idCarts)
        {
            try
            {
                float totalAmount = 0;
                var carts = await _context.Carts.Include(c => c.Variant).Where(c => idCarts.Contains(c.Id)).ToListAsync();
                foreach (var cart in carts)
                {
                    totalAmount += cart.Quanity * cart.Variant.Price;
                }
                return totalAmount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong in GetTotalAmountByCartsId");
            }
            return 0;
        }

        public async Task<CustomResult> GetUserCartQuantity(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            var quantity = _context.Carts.Where(c => c.UserId == user.Id).Count();

            return new CustomResult(200, "Success", quantity);
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

   


