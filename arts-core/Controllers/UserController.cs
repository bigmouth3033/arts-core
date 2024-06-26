using arts_core.Interfaces;
using arts_core.Models;
using arts_core.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace arts_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var users = _unitOfWork.UserRepository.GetAll();
            return Ok(users);
        }
        [HttpPost]
        public IActionResult Create()
        {
            var user = new User { Email = "nhan@gmail.com",Fullname = "Nguyen thanh nhan", Password="123", Address="434 LE loi",PhoneNumber = "424242",Avatar = "afa", Verifired = false, Active = false };
            _unitOfWork.UserRepository.CreateOwner(user);
            _unitOfWork.SaveChanges();
            return Ok("");
        }

        [HttpPost]
        [Route("create-employee")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateEmployee(CreateEmployee account)
        {
            var customResult = await _unitOfWork.UserRepository.CreateEmployee(account);

            _unitOfWork.SaveChanges();

            return Ok(customResult);
        }

        [HttpPost]
        [Route("create-customer")]
        public async Task<IActionResult> CreateCustomer([FromForm]CreateCustomer account)
        {
            var customResult = await _unitOfWork.UserRepository.CreateCustomer(account);

            _unitOfWork.SaveChanges();

            return Ok(customResult);
        }

        [HttpGet]
        [Route("get-employee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployees([FromQuery] int pageNumber, [FromQuery] int pageSize = 20)
        {
            var customPaging = await _unitOfWork.UserRepository.GetAllEmployees(pageNumber, pageSize);

            return Ok(customPaging);
        }

        [HttpPost]
        [Route("change-image")]
        [Authorize]
        public async Task<IActionResult> ChangeUserAvatar([FromForm] IFormFile image)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;

            var customResult = await _unitOfWork.UserRepository.ChangeUserImage(email, image);

            return Ok(customResult);
        }

        [HttpPost]
        [Route("change-info")]
        [Authorize]
        public async Task<IActionResult> ChangeUserInfo([FromForm] UpdateUserRequest info)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;

            var customResult = await _unitOfWork.UserRepository.EditUserInfo(email, info);

            return Ok(customResult);
        }
    }
}
