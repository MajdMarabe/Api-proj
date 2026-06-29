using KASHOP.BLL.Service;
using KASHOP.DAL.dto.request;
using KASHOP.DAL.Models;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {

        private readonly IOrderService _orderService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public OrdersController(IOrderService OrderService, IStringLocalizer<SharedResources> localizer)
        {

            _orderService = OrderService;
            _localizer = localizer;

        }
        [HttpGet("")]

        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetUserOrders(userId);
            return Ok(new
            {
                message = _localizer["Success"].Value,
                data = orders

            });
        }

        [HttpGet("{orderId}")]

        public async Task<IActionResult> GetOrder(int  orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderService.GetUserOrder(userId,orderId);
            return Ok(new
            {
                message = _localizer["Success"].Value,
                data = order

            });
        }
        [HttpGet("cancel/{orderId}")]

        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _orderService.CancelOrder(userId, orderId);
            if(!result) return Ok(new
            {
                message = _localizer["NotFound"].Value,

            });
            return Ok(new
            {
                message = _localizer["Success"].Value,

            });
        }


        [HttpGet("admin")]

        public async Task<IActionResult> GetAllOrders([FromQuery]OrderStatusEnum status = OrderStatusEnum.pending)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetAllOrders(status);
            return Ok(new
            {
                data = orders

            });
        }

        [HttpPatch("admin/{orderId}/status")]

        public async Task<IActionResult> ChangeOrderStatus(int orderId, [FromBody]ChangeOrderStatusRequest status )
        {
            var result = await _orderService.ChangeOrderStatus(orderId, status);
            if(!result) return BadRequest();
                return Ok();

         
        }
    }
}
