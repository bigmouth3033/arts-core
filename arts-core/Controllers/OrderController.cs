using arts_core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace arts_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("admin-orders")]
        [Authorize]
        public async Task<IActionResult> GetAdminOrders([FromQuery]int pageNumber, [FromQuery] int pageSize)
        {
            var customPaging = await _unitOfWork.OrderRepository.GetAllOrderAdmin(pageNumber, pageSize);

            return Ok(customPaging);
        }
    }
}
