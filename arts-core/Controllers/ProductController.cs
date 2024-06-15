using arts_core.Interfaces;
using arts_core.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace arts_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("new")]
        public IActionResult CreateProduct([FromForm]CreateProduct product)
        {
            var customResult = _unitOfWork.ProductRepository.CreateProduct(product);

            _unitOfWork.SaveChanges();

            return Ok(customResult);
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        [Route("admin-products")]
        public async Task<IActionResult> GetProducts([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var customPaging = await _unitOfWork.ProductRepository.GetPagingProducts(pageNumber, pageSize);

            return Ok(customPaging);
        }
    }
}
