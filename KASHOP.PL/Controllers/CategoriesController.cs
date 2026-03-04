using KASHOP.DAL.Data;
using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using KASHOP.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CategoriesController(ApplicationDbContext context, IStringLocalizer<SharedResources> localizer)
        {

            _context = context;
            _localizer = localizer;

        }

        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)//list of translations
        {
            var category = request.Adapt<Category>();
            _context.Add(category);
            _context.SaveChanges();
            return Ok(new
            {
                message = _localizer["Success"].Value

            });

        }
        [HttpGet("")]
        public IActionResult Get()
        {
            var categories = _context.categories.Include(c => c.Translations).ToList();
            var response = categories.Adapt<List<CategoryResponse>>();
            return Ok(new
            {
                message = _localizer["Success"].Value,
                data = response

            });
        }


    }
}