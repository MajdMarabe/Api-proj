using KASHOP.BLL.Service;
using KASHOP.DAL.dto.request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers() {
            var users = await _userManagementService.GetAllUsers();
            return Ok(users);
        }
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUser([FromRoute]string userId)
        {
            var user = await _userManagementService.GetUser(userId);
            return Ok(user);
        }
        [HttpPatch("{userId}/role")]
        public async Task<IActionResult> ChangeRole([FromRoute] string userId, [FromBody] ChangeRoleRequest request)
        {
            var result = await _userManagementService.ChangeRole(userId, request.newRole);
            if(!result) return BadRequest();
            return Ok();
        }


        [HttpPatch("{userId}/toggle-block")]
        public async Task<IActionResult> BlockToggle([FromRoute] string userId)
        {
            var result = await _userManagementService.ToggleBlockUser(userId);

            if (!result) return BadRequest();
            return Ok();
        }





    }
}
