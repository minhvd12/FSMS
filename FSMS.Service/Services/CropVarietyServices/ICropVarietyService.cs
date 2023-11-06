using FSMS.Service.ViewModels.CropVariety;
using FSMS.Service.ViewModels.Gardens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.CropVarietyServices
{
    public interface ICropVarietyService
    {
        Task<List<GetCropVariety>> GetAllAsync(string? varietyName = null, bool activeOnly = false);
        Task<GetCropVariety> GetAsync(int key);
        Task CreateCropVarietyAsync(CreateCropVariety createCropVariety);
        Task UpdateCropVarietyAsync(int key, UpdateCropVariety updateCropVariety);
        Task DeleteCropVarietyAsync(int key);
    }
}
