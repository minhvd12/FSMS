using FSMS.Service.ViewModels.ReviewFruits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.ReviewFruitServices
{
    public interface IReviewFruitService
    {
        Task<List<GetReviewFruit>> GetAllReviewFruitsAsync(bool activeOnly = false);
        Task<GetReviewFruit> GetAsync(int key);
        Task CreateReviewFruitAsync(CreateReviewFruit createReviewFruit);
        Task UpdateReviewFruitAsync(int key, UpdateReviewFruit updateReviewFruit);
        Task DeleteReviewFruitAsync(int key);
    }
}
