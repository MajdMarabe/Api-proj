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
        public async Task<IActionResult> Get()
        {
            var result = await _productService.GetAll();
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
    }

    }
