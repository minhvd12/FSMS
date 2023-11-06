using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.GardenRepositories;
using FSMS.Entity.Repositories.PostRepositories;
using FSMS.Entity.Repositories.RoleRepositories;
using FSMS.Entity.Repositories.UserRepositories;
using FSMS.Service.Enums;
using FSMS.Service.Services.FileServices;
using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.Posts;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.PostServices
{
    public class PostService : IPostService
    {
        private IUserRepository _userRepository;
        private IPostRepository _postRepository;
        private IRoleRepository _roleRepository;
        private readonly IFileService _fileService;

        private IMapper _mapper;
        public PostService(IUserRepository userRepository, IMapper mapper, IPostRepository postRepository,
            IRoleRepository roleRepository, IFileService fileService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _postRepository = postRepository;
            _roleRepository = roleRepository;
            _fileService = fileService;
        }

        public async Task CreatePostAsync(CreatePost createPost)
        {
            try
            {

                User existedUser = (await _userRepository.GetByIDAsync(createPost.UserId));
                if (existedUser == null)
                {
                    throw new Exception("UserId does not exist in the system.");
                }

                int lastId = (await _postRepository.GetAsync()).Max(x => x.PostId);
                Post post = new Post()
                {

                    PostTitle= createPost.PostTitle,
                    PostContent = createPost.PostContent,
/*                    PostImage = createPost.PostImage,
*/                    Type = createPost.Type,
                    UserId = createPost.UserId,
                    Status = StatusEnums.Active.ToString(),
                    CreatedDate = DateTime.Now,
                    PostId = lastId + 1
                };
                if (createPost.UploadFile == null)
                {
                    post.PostImage = "";
                }
                else if (createPost.UploadFile != null) post.PostImage = await _fileService.UploadFile(createPost.UploadFile);

                await _postRepository.InsertAsync(post);
                await _postRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task DeletePostAsync(int key)
        {
            try
            {
                Post existedPost = await _postRepository.GetByIDAsync(key);

                if (existedPost == null)
                {
                    throw new Exception("Post ID does not exist in the system.");
                }
                if (existedPost.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Post is not active.");
                }

                existedPost.Status = StatusEnums.InActive.ToString();

                await _postRepository.UpdateAsync(existedPost);
                await _postRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetPost> GetAsync(int key)
        {
            try
            {
                Post post = await _postRepository.GetByIDAsync(key);

                if (post == null)
                {
                    throw new Exception("Post ID does not exist in the system.");
                }
                if (post.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Post is not active.");
                }

                List<GetPost> posts = _mapper.Map<List<GetPost>>(
                    await _postRepository.GetAsync(includeProperties: "User")
                );

                GetPost result = _mapper.Map<GetPost>(post);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetPost>> GetAllAsync(string? postTitle = null, bool activeOnly = false)
        {
            try
            {
                List<GetPost> posts = _mapper.Map<List<GetPost>>(
                    (await _postRepository.GetAsync(includeProperties: "User"))
                    .Where(post =>
                        (string.IsNullOrEmpty(postTitle) || post.PostTitle.Contains(postTitle)) &&
                        (!activeOnly || post.Status == StatusEnums.Active.ToString()))
                    );

                return posts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task UpdatePostAsync(int key, UpdatePost updatePost)
        {
            try
            {
                Post existedPost = await _postRepository.GetByIDAsync(key);

                if (existedPost == null)
                {
                    throw new Exception("Post ID does not exist in the system.");
                }


                if (!string.IsNullOrEmpty(updatePost.PostTitle))
                {
                    existedPost.PostTitle = updatePost.PostTitle;
                }
                if (!string.IsNullOrEmpty(updatePost.PostContent))
                {
                    existedPost.PostContent = updatePost.PostContent;
                }
                /* if (!string.IsNullOrEmpty(updatePost.PostImage))
                 {
                     existedPost.PostImage = updatePost.PostImage;
                 }*/
                if (updatePost.UploadFile == null)
                {
                    existedPost.PostImage = "";
                }
                else if (updatePost.UploadFile != null) existedPost.PostImage = await _fileService.UploadFile(updatePost.UploadFile);


                if (!string.IsNullOrEmpty(updatePost.Type))
                {
                    existedPost.Type = updatePost.Type;
                }

                if (!string.IsNullOrEmpty(updatePost.Status))
                {
                    if (updatePost.Status != "Active" && updatePost.Status != "InActive")
                    {
                        throw new Exception("Status must be 'Active' or 'InActive'.");
                    }
                    existedPost.Status = updatePost.Status;
                }
                existedPost.UpdateDate = DateTime.Now;


                await _postRepository.UpdateAsync(existedPost);
                await _postRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
