using arts_core.Interfaces;
using arts_core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace arts_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartController> _logger;
        public CartController(IUnitOfWork unitOfWork, ILogger<CartController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetCarts()
        {
            int userId;
            string idClaim;
            List<Cart> carts;
            try
            {
                if (User.Claims.Any() && User.Claims != null)
                {
                    idClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
                    int.TryParse(idClaim, out userId);
                    carts = await _unitOfWork.CartRepository.GetCartsByUserIdAsync(userId);
                    if (carts == null || carts.Count == 0)
                    {
                        return Ok(new CustomResult(404, $"Nothing cart found by UserId {userId}", carts));
                    }
                    return Ok(new CustomResult(200, $"Cart found by UserId {userId}", carts));
                }
                else
                {
                    return Ok(new CustomResult(404, "UserClaim Null ", null));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong get cart by UserId  in cart controller");
            }
            return Ok("");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart(int userId, int variantId, int quanity)
        {
            try
            {
                var result = await _unitOfWork.CartRepository.CreateCartAsync(userId, variantId, quanity);
                if (result.isOkay)
                    return Ok(new CustomResult(201, $"{result.Message}", result));
                else
                    return Ok(new CustomResult(405, $"Create cartId fail", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong when CreateCart in CartController");
            }
            return Ok("smt");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCart(int cartId, int quanity)
        {
            try
            {
                var result = await _unitOfWork.CartRepository.UpdateCartById(cartId, quanity);
                if (result.isOkay)
                    return Ok(new CustomResult(201, $"Update cartId {cartId} success", result));
                else
                    return Ok(new CustomResult(405, $"Update cartId {cartId} fail", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot update cartId {cartId}", cartId);
            }
            return Ok("");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCart([FromQuery] int cartId)
        {
            try
            {
                var result = await _unitOfWork.CartRepository.DeleteCartById(cartId);
                return Ok(new CustomResult(200, $"{result.Message}", result));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public static class Extension
    {
        public static List<T> Paginate<T>(this List<T> records, int pageNumber = 1, int pageSize = 10)
        {
            return records.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
