using FSMS.Service.ViewModels.Plants;
using FSMS.Service.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.PostServices
{
    public interface IPostService
    {
        Task<List<GetPost>> GetAllAsync(string? postTitle = null, bool activeOnly = false);
        Task<GetPost> GetAsync(int key);
        Task CreatePostAsync(CreatePost createPost);
        Task UpdatePostAsync(int key, UpdatePost updatePost);
        Task DeletePostAsync(int key);

    }
}
