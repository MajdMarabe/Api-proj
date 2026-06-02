using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface IAuthenticationService
    {
        public Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        public Task<LoginResponse> LoginAsync(LoginRequest request);
        public Task<bool> ConfirmEmailAsync(string token, string id);
        public Task<ForgetPasswordResponse> RequestResetPasswordAsync(ForgetPasswordRequest request);
        public Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
