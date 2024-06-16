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
        public async Task<IActionResult> GetCarts(int userId)
        {
            try
            {
                var carts = await _unitOfWork.CartRepository.GetCartsByUserIdAsync(userId);
                if (carts == null || carts.Count == 0)
                {
                    return Ok(new CustomResult(404, $"Nothing cart found by UserId {userId}", carts));
                }
                return Ok(new CustomResult(200, $"Cart found by UserId {userId}", carts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong get cart by UserId {userId} in cart controller", userId);
            }
            return Ok("");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart(int userId, int variantId, int quanity)
        {
            try
            {
                await _unitOfWork.CartRepository.CreateCartAsync(userId, variantId, quanity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong when CreateCart in CartController");
            }
            return Ok("");
        }
    }
}
