using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
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

            return new LoginResponse() { Success = true, Message = "succes" };

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
                $"http://localhost:5203/api/Account/confirmEmail?token={token}&id={user.Id}";


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
