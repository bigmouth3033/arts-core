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
        public IActionResult CreateProduct([FromForm] CreateProduct product)
        {
            var customResult = _unitOfWork.ProductRepository.CreateProduct(product);

            _unitOfWork.SaveChanges();

            return Ok(customResult);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        [Route("admin-products")]
        public async Task<IActionResult> GetProducts([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] IEnumerable<int> categoryId, [FromQuery] string searchValue ="")
        {
            var customPaging = await _unitOfWork.ProductRepository.GetPagingProducts(pageNumber, pageSize, categoryId,searchValue);

            return Ok(customPaging);
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct([FromQuery]int id)
        {
            var customResult = await _unitOfWork.ProductRepository.GetProduct(id);

            return Ok(customResult);
        }

        [HttpGet]
        [Route("admin")]
        public async Task<IActionResult> GetProductAdmin([FromQuery] int id)
        {
            var customResult = await _unitOfWork.ProductRepository.GetProductAdmin(id);

            return Ok(customResult);
        }


        [HttpGet]
        [Route("product-variant")]
        public async Task<IActionResult> GetProductVariantDetail([FromQuery] int id)
        {
            var customResult = await _unitOfWork.ProductRepository.GetProductVariantInfo(id);

            return Ok(customResult);
        }

        [HttpPost]
        [Route("add-images")]
        public async Task<IActionResult> CreateImages([FromForm]RequestImages images)
        {
            var customResult = await _unitOfWork.ProductRepository.CreateImages(images);

            _unitOfWork.SaveChanges();

            return Ok(customResult);
        }

        [HttpDelete]
        [Route("remove-image")]
        public async Task<IActionResult> RemoveImage([FromQuery] int imageId)
        {
            var customResult = await _unitOfWork.ProductRepository.DeleteImage(imageId);

            _unitOfWork.SaveChanges();

            return Ok(customResult);
        }

        [HttpGet]
        [Route("listing-page")]
        public async Task<IActionResult> GetPagingProductForListingPage([FromQuery] int categoryId, [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] int sort, [FromQuery] string searchValue = "", [FromQuery] float priceRangeMin = 0, [FromQuery] float priceRangeMax = float.MaxValue)
        {
            var customPaging = await _unitOfWork.ProductRepository.GetPagingProductForListingPage(categoryId, pageNumber, pageSize, sort, searchValue, priceRangeMin, priceRangeMax);

            return Ok(customPaging);
        }
    }
}
