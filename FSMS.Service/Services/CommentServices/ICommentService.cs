using FSMS.Service.ViewModels.Comments;
using FSMS.Service.ViewModels.CropVariety;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.CommentServices
{
    public interface ICommentService
    {
        Task<List<GetComment>> GetAllAsync(bool activeOnly = false);
        Task<GetComment> GetAsync(int key);
        Task CreateCommentAsync(CreateComment createComment);
        Task UpdateCommentAsync(int key, UpdateComment updateComment);
        Task DeleteCommentAsync(int key);
    }
    
}
