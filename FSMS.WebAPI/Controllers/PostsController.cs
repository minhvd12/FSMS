using FSMS.Service.Services.GardenServices;
using FSMS.Service.Services.PostServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Validations;
using FSMS.Service.Validations.Post;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private IPostService _postService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public PostsController(IPostService postService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _postService = postService;
            _jwtAuthOptions = jwtAuthOptions;
        }
        [HttpGet]
        [PermissionAuthorize("Expert", "Farmer")]
        public async Task<IActionResult> GetAllPosts(string? postTitle = null, bool activeOnly = false)
        {
            try
            {
                List<GetPost> posts = await _postService.GetAllAsync(postTitle, activeOnly);
                return Ok(new
                {
                    Data = posts
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
        [PermissionAuthorize("Expert", "Farmer")]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                GetPost post = await _postService.GetAsync(id);
                return Ok(new
                {
                    Data = post
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
     /*   [PermissionAuthorize("Expert")]*/
        public async Task<IActionResult> CreatePost([FromForm] CreatePost createPost)
        {
            var validator = new PostValidator();
            var validationResult = validator.Validate(createPost);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                await _postService.CreatePostAsync(createPost);

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
        [PermissionAuthorize("Expert")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] UpdatePost updatePost)
        {
            var validator = new UpdatePostValidator();
            var validationResult = validator.Validate(updatePost);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _postService.UpdatePostAsync(id, updatePost);
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
        [PermissionAuthorize("Expert")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                await _postService.DeletePostAsync(id);
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
