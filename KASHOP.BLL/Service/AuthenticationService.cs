using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)            
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) return new LoginResponse() { Success = false, Message = "invalid email" };
            /////
            var confirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!confirmed) return new LoginResponse() { Success = false, Message = "Please confirm your email" };
            ////
            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result) return new LoginResponse() { Success = false, Message = "invalid Password" };

            return new LoginResponse() { Success = true, Message = "succes", AccessToken = await GenerateAccessToken(user) };

        }   
        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)

            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token =new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return new RegisterResponse()
                {
                    Success = false,
                    Message = "Error"
                };

            await _userManager.AddToRoleAsync(user, "User");

            var token =
                await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);
            var url =
                $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Account/confirmEmail?token={token}&id={user.Id}";


            await _emailSender.SendEmailAsync(
                user.Email,
                "Welcome",
                $"<h1>Welcome {request.UserName}</h1>" +
                $"<a href='{url}'>Confirm Email</a>"
            );
            return new RegisterResponse()
            {
                Success = true,
                Message = "success"
            };
        }
        public async Task<bool> ConfirmEmailAsync( string token, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return false;
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(!result.Succeeded) return false;
            return true;

        }
    }
}
