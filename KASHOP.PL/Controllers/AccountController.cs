using KASHOP.BLL.Service;
using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;

        }
        [HttpPost("register")]
        public async Task<IActionResult> AddUser(RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);

        }

        [HttpGet ("confirmEmail")]
         public async Task<IActionResult> ConfirmEmail()
        {

            return Ok(new { message = "ok" });
        }



    }
}
