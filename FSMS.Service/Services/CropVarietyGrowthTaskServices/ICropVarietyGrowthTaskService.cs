using FSMS.Service.ViewModels.CropVarietyGrowthTasks;
using FSMS.Service.ViewModels.GardenTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.CropVarietyGrowthTaskServices
{
    public interface ICropVarietyGrowthTaskService
    {
        Task<List<GetCropVarietyGrowthTask>> GetAllAsync(string? taskName = null, DateTime? startDate = null, bool activeOnly = false);
        Task<GetCropVarietyGrowthTask> GetAsync(int key);
        Task CreateCropVarietyGrowthTaskAsync(CreateCropVarietyGrowthTask createCropVarietyGrowthTask);
        Task UpdateCropVarietyGrowthTaskAsync(int key, UpdateCropVarietyGrowthTask updateCropVarietyGrowthTask);
        Task DeleteCropVarietyGrowthTaskAsync(int key);
    }
}
