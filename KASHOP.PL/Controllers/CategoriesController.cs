using KASHOP.BLL.Service;
using KASHOP.DAL.Data;
using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using KASHOP.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Reflection.Metadata.Ecma335;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _CategoryService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CategoriesController(ICategoryService CategoryService, IStringLocalizer<SharedResources> localizer)
        {

            _CategoryService = CategoryService;
            _localizer = localizer;

        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> Create(CategoryRequest request)//list of translations
        {
            await _CategoryService.Create(request);
            return Ok(new
            {
                message = _localizer["Success"].Value

            });

        }
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var categories = await _CategoryService.GetAll();
            return Ok(new
            {
                message = _localizer["Success"].Value,
                data = categories

            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok( await _CategoryService.GetCategory(c=>c.Id == id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id )
        {
            var deleted = await _CategoryService.DeleteCategory(id);
            if (!deleted) return NotFound(new {messege = _localizer["NotFound"].Value });
            return Ok(new {message = _localizer["Success"].Value });
        }
         


    }
}