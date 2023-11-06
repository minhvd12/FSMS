using FSMS.Service.ViewModels.FruitDiscounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.FruitDiscountSevices
{
    public interface IFruitDiscountService
    {
        Task<List<GetFruitDiscount>> GetAllAsync(string? discountName = null, DateTime? discountExpiryDate = null, bool activeOnly = false);
        Task<GetFruitDiscount> GetAsync(int key);
        Task CreateFruitDiscountAsync(CreateFruitDiscount createFruitDiscount);
        Task UpdateFruitDiscountAsync(int key, UpdateFruitDiscount updateFruitDiscount);
        Task DeleteFruitDiscountAsync(int key);
    }
}
