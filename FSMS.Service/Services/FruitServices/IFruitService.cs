using FSMS.Service.ViewModels.Fruits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.FruitServices
{
    public interface IFruitService
    {
        Task<List<GetFruitFarmer>> GetAllFruitFarmerAsync(string? fruitName = null, decimal? minPrice = null, decimal? maxPrice = null, bool activeOnly = false, DateTime? createdDate = null, DateTime? newestDate = null);
        Task<GetFruitFarmer> GetFruitFarmerAsync(int key);
        Task CreateFruitFarmerAsync(CreateFruitFarmer createFruitFarmer);
        Task UpdateFruitFarmerAsync(int key, UpdateFruitFarmer updateFruitFarmer);
        Task DeleteFruitFarmerAsync(int key);

        Task<List<GetFruitSupplier>> GetAllFruitSupplierAsync(string? fruitName = null, decimal? minPrice = null, decimal? maxPrice = null, bool activeOnly = false, DateTime? createdDate = null, DateTime? newestDate = null);
        Task<GetFruitSupplier> GetFruitSupplierAsync(int key);
        Task CreateFruitSupplierAsync(CreateFruitSupplier createFruitSupplier);
        Task UpdateFruitSupplierAsync(int key, UpdateFruitSupplier updateFruitSupplier);
        Task DeleteFruitSupplierAsync(int key);
    }
}
