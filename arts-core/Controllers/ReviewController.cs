using arts_core.Interfaces;
using arts_core.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace arts_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public ReviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromForm] RequestReview review)
        {
            string idClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            int userId;
            int.TryParse(idClaim, out userId);

            var reviewResult = _unitOfWork.ReviewRepository.CreateReview(userId, review);

            _unitOfWork.SaveChanges();

            return Ok(reviewResult);
        }

    }
}
