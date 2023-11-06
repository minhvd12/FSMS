using FSMS.Service.ViewModels.CropVarietyGrowthTasks;
using FSMS.Service.ViewModels.CropVarietyStages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.CropVarietyStageServices
{
    public interface ICropVarietyStageService
    {
        Task<List<GetCropVarietyStage>> GetAllAsync(string? stageName = null, DateTime? startDate = null, bool activeOnly = false);
        Task<GetCropVarietyStage> GetAsync(int key);
        Task CreateCropVarietyStageAsync(CreateCropVarietyStage createCropVarietyStage);
        Task UpdateCropVarietyStageAsync(int key, UpdateCropVarietyStage updateCropVarietyStage);
        Task DeleteCropVarietyStageAsync(int key);
    }
}
