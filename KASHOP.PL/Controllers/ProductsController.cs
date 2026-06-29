using KASHOP.BLL.Service;
using KASHOP.DAL.dto.request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProductRequest request)
        {
            var result = await _productService.Create(request);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProductFilterRequest request)
        {
            var result = await _productService.GetAll(request);
            return Ok(new { Products = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetProduct(p => p.Id == id);
            if (result == null)
                return NotFound();
            return Ok(new { Product = result });
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteProduct(id);
            if (!deleted) return BadRequest();
            return Ok(new { message = "Product deleted successfully" });
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request, int id)
        {
            var updated = await _productService.UpdateProduct(request, id);
            if (!updated) return BadRequest();
            return Ok(new { message = "Product updated successfully" });
        }

        [HttpPatch("{id}/status")]
        [Authorize]
        public async Task<IActionResult> ChangeStatuse(int id)
        {
            var updated = await _productService.ToggleStatuse(id);
            if (!updated) return BadRequest();
            return Ok(new { message = "status updated successfully" });
        }


    }
}
