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
        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            _userManager=userManager;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
             var user =  await _userManager.FindByEmailAsync(request.Email);
            if (user is null) return new LoginResponse() { Success = false , Message="invalid email"};

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if(!result) return new LoginResponse() { Success = false , Message="invalid Password"};

            return new LoginResponse() { Success = true, Message = "succes" };

        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user =  request.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
              return new RegisterResponse() { Success = false , Message="Error"};
           return new RegisterResponse() { Success = true, Message = "success" };

        }
    }
}
