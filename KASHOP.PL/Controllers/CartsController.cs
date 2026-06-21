using KASHOP.BLL.Service;
using KASHOP.DAL.dto.request;
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

    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CartsController(ICartService cartService, IStringLocalizer<SharedResources> localizer)
        {
            _cartService = cartService;
            _localizer = localizer;


        }

        [HttpPost("")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.AddToCart(request, userId);
            if (!result)
            {
                return BadRequest();
            }
            return Ok(
                new
                {
                    message = _localizer["Success"].Value,
                }

                );
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _cartService.GetCartItems(userId);
            return Ok(new { data = items });

        }

        //cart/1
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.RemoveItem(productId, userId);
            if (result) return Ok(new { message = _localizer["Success"].Value });
            return BadRequest();


        }
        [HttpDelete("")]
        public async Task<IActionResult> DeleteItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.ClearCart(userId);
            if (result) return Ok(new { message = _localizer["Success"].Value });
            return BadRequest();


        }
        [HttpPatch("{productId}")]
        public async Task<IActionResult> UpdateQuantity([FromRoute] int productId, [FromBody] UpdateCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.UpdateQantity(productId, request.Count, userId);
            if (result) return Ok(new { message = _localizer["Success"].Value });
            return BadRequest();


        }
    }
}
