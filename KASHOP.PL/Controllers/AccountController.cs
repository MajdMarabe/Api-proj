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
         public async Task<IActionResult> ConfirmEmail( string token, string id)
        {
             var result = await _authenticationService.ConfirmEmailAsync( token, id);
            if (!result) return BadRequest(new { message = "Invalid token or user ID" });
            return Ok(new { message = "Email confirmed successfully" });
        }

        [HttpPost("sendcode")]
        public async Task<IActionResult> ResetPassword(ForgetPasswordRequest request)
        {
        var result  = await _authenticationService.RequestResetPasswordAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);

        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var result = await _authenticationService.ResetPasswordAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }



    }
}
