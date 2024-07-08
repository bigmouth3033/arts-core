using arts_core.Interfaces;
using arts_core.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace arts_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("admin-orders")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> GetAdminOrders([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string active, [FromQuery] string orderId = "",
          [FromQuery] List<string> category = null,
          [FromQuery] string customer = "",
          [FromQuery] string productCode = "",
          [FromQuery] List<string> payment = null,
          [FromQuery] string paymentCode = "",
          [FromQuery] List<string> delivery = null,
          [FromQuery] string from = "",
          [FromQuery] string to = "",
          [FromQuery] string fromDate = "",
          [FromQuery] string toDate = ""
          )
        {
            var customPaging = await _unitOfWork.OrderRepository.GetAllOrderAdmin(pageNumber, pageSize, active , orderId,
              customer,
              category,
              productCode,
              payment,
              paymentCode,
              delivery,
              from,
              to, fromDate,toDate);

            return Ok(customPaging);
        }

        [HttpPost]
        [Route("accept-order")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> AcceptOrder([FromForm] ICollection<int> orderId)
        {
            var customResult = await _unitOfWork.OrderRepository.AcceptOrder(orderId);

            return Ok(customResult);
        }

        [HttpPost]
        [Route("deny-order")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> DenyOrder([FromForm] ICollection<int> orderId)
        {
            var customResult = await _unitOfWork.OrderRepository.DenyOrder(orderId);

            return Ok(customResult);
        }

        [HttpPost]
        [Route("delivery-order")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> DeliveryOrder([FromForm] ICollection<int> orderId)
        {
            var customResult = await _unitOfWork.OrderRepository.DeliveryOrder(orderId);

            return Ok(customResult);
        }

        [HttpPost]
        [Route("finish-order")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> FinishOrder([FromForm] ICollection<int> orderId)
        {
            var customResult = await _unitOfWork.OrderRepository.OrderSuccess(orderId);

            return Ok(customResult);
        }

        [HttpGet]
        [Route("get-admin-order")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> GetAdminOrder([FromQuery] int orderId)
        {
            var customResult = await _unitOfWork.OrderRepository.OrderDetail(orderId);

            return Ok(customResult);
        }

        [HttpGet]
        [Route("get-customer-order")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCustomerOrder([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string active, string search = "")
        {
            int userId;
            string idClaim;
            idClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            int.TryParse(idClaim, out userId);

            var customPaging = await _unitOfWork.OrderRepository.GetCustomerOrders(userId, pageNumber, pageSize, active, search);

            return Ok(customPaging);
        }

        [HttpGet]
        [Route("get-customer-order-detail")]
        [Authorize]
        public async Task<IActionResult> GetCustomerOrder([FromQuery] int orderId)
        {
            int userId;
            string idClaim;
            idClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            int.TryParse(idClaim, out userId);

            var customrResult = await _unitOfWork.OrderRepository.GetOrderDetail(userId, orderId);

            return Ok(customrResult);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [Route("cancel-order")]
        public async Task<IActionResult> CancelOrder([FromForm] RequestCancel requestCancel)
        {
            int userId;
            string idClaim;
            idClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            int.TryParse(idClaim, out userId);

            var customrResult = await _unitOfWork.OrderRepository.CancelOrder(userId, requestCancel.OrderId, requestCancel.Reason);

            return Ok(customrResult);
        }

        [HttpGet]
        [Route("order-refund")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> GetOrderRefund([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string active)
        {
        
            var customPaging = await _unitOfWork.OrderRepository.GetOrderRefund(pageNumber, pageSize, active);

            return Ok(customPaging);
        }

        [HttpGet]
        [Route("exchange")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> GetExchange([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string active)
        {

            var customPaging = await _unitOfWork.OrderRepository.GetOrderExchage(pageNumber, pageSize, active);

            return Ok(customPaging);
        }

        [HttpGet]
        [Route("monthly-revenue")]
        [Authorize]
        public async Task<IActionResult> GetMonthlyRevenue(int year) {
            var customResult = await _unitOfWork.OrderRepository.GetMonthlyRevenue(year);

            return Ok(customResult);
        }

        [HttpGet]
        [Route("stream-revenue")]
        [Authorize]
        public async Task<IActionResult> GetStreamRevenue([FromQuery]string option)
        {
            var customResult = await _unitOfWork.OrderRepository.GetOrderRevenue(option);

            return Ok(customResult);
        }

        [HttpGet]
        [Route("stream-order-time")]
        [Authorize]
        public async Task<IActionResult> GetStreamNumberOfOrder([FromQuery] string option)
        {
            var customResult = await _unitOfWork.OrderRepository.GetNumberOfOrder(option);

            return Ok(customResult);
        }

        [HttpGet]
        [Route("recent-order")]
        [Authorize]
        public async Task<IActionResult> GetRecentOrder()
        {
            var customResult = await _unitOfWork.OrderRepository.GetRecentOrder();

            return Ok(customResult);
        }

      
        [HttpGet]
        [Route("order-dashboard")]
        [Authorize]
        public IActionResult GetOrderDashBoard()
        {
            var customResult =  _unitOfWork.OrderRepository.GetOrderDashBoard();

            return Ok(customResult);
        }

        [HttpGet]
        [Route("user-exchange")]
        [Authorize]
        public async Task<IActionResult> GetUserRefundExchange([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string active)
        {
            int userId;
            string idClaim;
            idClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            int.TryParse(idClaim, out userId);
            var customPaging = await _unitOfWork.OrderRepository.GetUserRefundExchange(userId, pageNumber, pageSize, active);

            return Ok(customPaging);
        }
    }
}
