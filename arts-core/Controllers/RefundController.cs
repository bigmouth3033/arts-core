using arts_core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace arts_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RefundController> _logger;
        public RefundController(IUnitOfWork unitOfWork,ILogger<RefundController> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRefund(RefundRequest refundRequest)
        {
            var result = await _unitOfWork.RefundRepository.CreateRefundAsync(refundRequest);
            return Ok(result);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetRefundsByUserId(int userId)
        {


            var result = await _unitOfWork.RefundRepository.GetRefundsByUserIdAsync(userId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRefundsForAdmin()
        {
            var result = await _unitOfWork.RefundRepository.GetAllRefundsAsync();
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRefundForAdmin(RefundReQuestForAdmin request)
        {
            var result =await _unitOfWork.RefundRepository.UpdateRefundAsync(request);
            return Ok(result);
        }
    }
}
