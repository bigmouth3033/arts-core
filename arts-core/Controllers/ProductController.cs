﻿using arts_core.Interfaces;
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
        public async Task<IActionResult> GetProducts([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var customPaging = await _unitOfWork.ProductRepository.GetPagingProducts(pageNumber, pageSize);

            return Ok(customPaging);
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct([FromQuery]int id)
        {
            var customResult = await _unitOfWork.ProductRepository.GetProduct(id);

            return Ok(customResult);
        }

        [HttpGet]
        [Route("product-variant")]
        public async Task<IActionResult> GetProductVariantDetail([FromQuery] int id)
        {
            var customResult = await _unitOfWork.ProductRepository.GetProductVariantInfo(id);

            return Ok(customResult);
        }

        
    }
}
