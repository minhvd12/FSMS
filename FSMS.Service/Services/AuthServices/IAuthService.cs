using FSMS.Service.ViewModels.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.AuthServices
{
    public interface IAuthService
    {
        Task<SignInAccount> SignInAsync(Account account, JwtAuth jwtAuth);
        Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken, JwtAuth jwtAuth);
        Task<bool> SendPasswordResetEmailAndCheckUserAsync(string userEmail);
        string GenerateOTP();
        Task<bool> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
        Task<bool> LogoutAsync(string userEmail, SignInAccount signInAccount);
        Task RegisterAccountAsync(RegisterAccount registerAccount);




    }
}
