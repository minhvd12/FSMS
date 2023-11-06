using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Utility;
using FSMS.Service.ViewModels.Authentications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using FSMS.Service.Services.UserServices;
using FSMS.Service.ViewModels.Users;
using FSMS.Service.Validations.User;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public UsersController(IUserService userService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _userService = userService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetAllUsers(string? fullNameCustomer = null, bool activeOnly = false, string? roleName = null)
        {
            try
            {
                List<GetUser> users = await _userService.GetAllAsync(fullNameCustomer, activeOnly, roleName);

                return Ok(new
                {
                    Data = users
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpGet("{id}")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                GetUser user = await _userService.GetAsync(id);
                return Ok(new
                {
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpPost]
        //[PermissionAuthorize("Admin")]
        public async Task<IActionResult> CreateUser([FromForm] CreateUser createUser)
        {
            var validator = new UserValidator();
            var validationResult = validator.Validate(createUser);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                await _userService.CreateUserAsync(createUser);

                return Ok();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpPut("{id}")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UpdateUser updateUser)
        {
            var validator = new UpdateValidator();
            var validationResult = validator.Validate(updateUser);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _userService.UpdateUserAsync(id, updateUser);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [PermissionAuthorize("Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
    }
}
