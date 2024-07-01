using arts_core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace arts_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExchangeController> _logger;

        public ExchangeController(ILogger<ExchangeController> logger, IUnitOfWork unitOfWork )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

        }

        [HttpPost]
        public async Task<IActionResult> CreateExchangeForClient([FromForm]ExchangeRequest request)
        {
            var result = await _unitOfWork.ExchangeRepository.CreateExchangeAsync(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEchangeForAdmin(UpdateExchangeRequest request)
        {
            var result = await _unitOfWork.ExchangeRepository.UpdateExchangeAsync(request);
            return Ok(result);
        }
    }
}
