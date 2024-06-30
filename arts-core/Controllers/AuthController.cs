using arts_core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace arts_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public AuthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("admin-login")]
        public async Task<IActionResult> AdminLogin([FromForm] LoginRequest account)
        {
            var customResult = await _unitOfWork.UserRepository.AdminLogin(account);

            return Ok(customResult);
        }

        [HttpPost]
        [Route("customer-login")]
        public async Task<IActionResult> Customer([FromForm] LoginRequest account)
        {
            var customResult = await _unitOfWork.UserRepository.CustomerLogin(account);

            return Ok(customResult);
        }

        [HttpGet]
        [Route("customer")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCustomer()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;

            var customResult = await _unitOfWork.UserRepository.GetUser(email);

            return Ok(customResult);
        }

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles ="Admin,Employee")]
        public async Task<IActionResult> GetAdmin()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;

            var customResult = await _unitOfWork.UserRepository.GetAdmin(email);

            return Ok(customResult);
        }

        [HttpPut]
        [Route("verify")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> VerifyAccount()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var customResult = await _unitOfWork.UserRepository.VerifyAccount(email);

            _unitOfWork.SaveChanges();

            return Ok(customResult);
        }
    }
}
