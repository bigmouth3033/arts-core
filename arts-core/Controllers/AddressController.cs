using arts_core.Interfaces;
using arts_core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace arts_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {

        private IUnitOfWork _unitOfWork;
        public AddressController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewAddress([FromForm] Address address)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;

            var customResult = await _unitOfWork.AddressRepository.CreateNewAddress(email, address);

            return Ok(customResult);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;

            var customResult = await _unitOfWork.AddressRepository.GetUserAddress(email);

            return Ok(customResult);
        }
    }
}
