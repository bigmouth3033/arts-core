using arts_core.Interfaces;
using arts_core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
