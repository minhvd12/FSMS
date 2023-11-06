using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.RoleRepositories;
using FSMS.Entity.Repositories.UserRepositories;
using FSMS.Service.Enums;
using FSMS.Service.Services.FileServices;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.ViewModels.Authentications;
using Google.Apis.Auth.OAuth2;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace FSMS.Service.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private IMapper _mapper;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IFileService _fileService;

        private static Dictionary<string, string> otpStorage = new Dictionary<string, string>();

        public AuthService(IMapper mapper, IUserRepository userRepository, IRoleRepository roleRepository, IFileService fileService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _fileService = fileService;
        }


        public async Task<SignInAccount> SignInAsync(Account account, JwtAuth jwtAuth)
        {
            try
            {
                User existedAccount = (await _userRepository.GetAsync()).FirstOrDefault(x => x.Email == account.Email && x.Password.Equals(account.Password));

                if (existedAccount == null)
                {
                    throw new Exception("Email or Password is invalid.");
                }

                if (existedAccount.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Your account is not active.");
                }

                // Lấy thông tin vai trò của người dùng
                Role userRole = await _roleRepository.GetByIDAsync(existedAccount.RoleId);

                SignInAccount getUser = _mapper.Map<SignInAccount>(existedAccount);

                getUser.RoleName = userRole.RoleName;
                getUser.RoleId = userRole.RoleId;

                // Tạo access token
                getUser.AccessToken = GenerateAccessToken(getUser.Email, getUser.UserId.ToString(), getUser.RoleName, jwtAuth);
                getUser.RefreshToken = GenerateRefreshToken(getUser.Email, getUser.UserId.ToString(), getUser.RoleName, jwtAuth);


                return getUser;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RegisterAccountAsync(RegisterAccount registerAccount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(registerAccount.FullName) || string.IsNullOrWhiteSpace(registerAccount.Password) ||
                    string.IsNullOrWhiteSpace(registerAccount.Email) || string.IsNullOrWhiteSpace(registerAccount.PhoneNumber))
                {
                    throw new Exception("Invalid registration data. All required fields must be provided.");
                }

                if (await IsEmailAlreadyRegisteredAsync(registerAccount.Email))
                {
                    throw new Exception("Email is already registered.");
                }
                int lastUserId = (await _userRepository.GetAsync()).OrderBy(x => x.UserId).Last().UserId;

                var user = new User
                {
                    FullName = registerAccount.FullName,
                    Password = registerAccount.Password,
                    Email = registerAccount.Email,
                    PhoneNumber = registerAccount.PhoneNumber,
                    Address = registerAccount.Address,
                    /*                    ImageMomoUrl = registerAccount.ImageMomoUrl,
                                        ProfileImageUrl = registerAccount.ProfileImageUrl,*/
                    RoleId = registerAccount.RoleId,
                    Status = StatusEnums.Active.ToString(),
                    UserId = lastUserId + 1

                };
                if (registerAccount.ImageMomoUrl == null)
                {
                    user.ImageMomoUrl = "";
                }
                else if (registerAccount.ImageMomoUrl != null) user.ImageMomoUrl = await _fileService.UploadFile(registerAccount.ImageMomoUrl);
                if (registerAccount.ProfileImageUrl == null)
                {
                    user.ProfileImageUrl = "";
                }
                else if (registerAccount.ProfileImageUrl != null) user.ProfileImageUrl = await _fileService.UploadFile(registerAccount.ProfileImageUrl);

                await _userRepository.InsertAsync(user);
                await _userRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to register the account: " + ex.Message);
            }
        }

        private async Task<bool> IsEmailAlreadyRegisteredAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);
            return user != null;
        }





        public async Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken, JwtAuth jwtAuth)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuth.Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                if (!(validatedToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                var emailClaim = principal.FindFirst(ClaimTypes.Email);
                var userIdClaim = principal.FindFirst(ClaimTypes.Name);
                var roleClaim = principal.FindFirst("Role");

                if (emailClaim == null || userIdClaim == null || roleClaim == null)
                {
                    throw new SecurityTokenException("Invalid token claims");
                }

                var userEmail = emailClaim.Value;
                var userId = userIdClaim.Value;
                var userRole = roleClaim.Value;

                var newRefreshToken = GenerateRefreshToken(userEmail, userId, userRole, jwtAuth);

                return new RefreshTokenResponse
                {
                    RefreshToken = newRefreshToken,
                    NewRefreshToken = refreshToken
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GenerateRefreshToken(string userEmail, string userId, string userRole, JwtAuth jwtAuth)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuth.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Email, userEmail),
                 new Claim(JwtRegisteredClaimNames.Name, userId),
                 new Claim("Role", userRole),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials,
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }


        public async Task<bool> SendPasswordResetEmailAndCheckUserAsync(string userEmail)
        {
            try
            {
                var user = await GetUserByEmailAsync(userEmail);
                if (user != null)
                {
                    var otp = GenerateOTP();

                    otpStorage[userEmail] = otp;

                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential("duongkhaiduy5@gmail.com", "vdob zizq mrvj ravs"),
                        EnableSsl = true,
                    };

                    var from = new MailAddress("duongkhaiduy5@gmail.com", "Fruit Season Management");
                    var to = new MailAddress(userEmail);
                    var subject = "Password Reset OTP";
                    var body = $"Your OTP is: {otp}";

                    using (var mailMessage = new MailMessage(from, to)
                    {
                        Subject = subject,
                        Body = body,
                    })
                    {
                        smtpClient.Send(mailMessage);
                    }

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                var user = await GetUserByEmailAsync(resetPasswordRequest.Email);
                if (user != null)
                {
                    // Truy xuất OTP từ otpStorage bằng địa chỉ email
                    if (otpStorage.TryGetValue(resetPasswordRequest.Email, out var storedOtp) && resetPasswordRequest.OTP == storedOtp)
                    {
                        Console.WriteLine(otpStorage);
                        user.Password = resetPasswordRequest.Password;

                        await _userRepository.UpdateAsync(user);

                        // Xóa OTP sau khi đã sử dụng
                        otpStorage.Remove(resetPasswordRequest.Email);

                        return true;
                    }
                    else
                    {
                        throw new Exception("Invalid OTP");
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        private SignInAccount GenerateToken(SignInAccount getUser, JwtAuth jwtAuth)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuth.Key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                 {
                     new Claim(JwtRegisteredClaimNames.Email, getUser.Email),
                     new Claim(JwtRegisteredClaimNames.Name, getUser.FullName),
                     new Claim("Role", getUser.RoleName),
                     new Claim("RoleId", getUser.RoleId.ToString()),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                 };

                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = credentials,
                };

                var token = jwtTokenHandler.CreateToken(tokenDescription);
                string accessToken = jwtTokenHandler.WriteToken(token);

                getUser.AccessToken = accessToken;

                return getUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GenerateAccessToken(string userEmail, string userId, string userRole, JwtAuth jwtAuth)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuth.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, userEmail),
                new Claim(JwtRegisteredClaimNames.Name, userId),
                new Claim("Role", userRole),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials,
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> GetUserByEmailAsync(string userEmail)
        {
            try
            {
                var user = (await _userRepository.GetAsync()).FirstOrDefault(x => x.Email == userEmail);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string GenerateOTP()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[4];
                rng.GetBytes(randomBytes);
                int otpValue = Math.Abs(BitConverter.ToInt32(randomBytes, 0)) % 1000000;
                return otpValue.ToString("D6");
            }
        }
        public async Task<bool> LogoutAsync(string userEmail, SignInAccount signInAccount)
        {
            try
            {
                var user = await GetUserByEmailAsync(userEmail);

                if (user != null)
                {
                    signInAccount.AccessToken = null;



                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
