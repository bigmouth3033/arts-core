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

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetAdmin()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;

            var customResult = _unitOfWork.UserRepository.GetAdmin(email);

            return Ok(customResult);
        }
    }
}
