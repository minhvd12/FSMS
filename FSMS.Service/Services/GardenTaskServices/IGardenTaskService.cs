using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.GardenTasks;
using FSMS.Service.ViewModels.Seasons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.GardenTaskServices
{
    public interface IGardenTaskService
    {
        Task<List<GetGardenTask>> GetAllAsync(string? gardenTaskName = null, DateTime? taskDate = null, bool activeOnly = false);
        Task<GetGardenTask> GetAsync(int key);
        Task CreateGardenTaskAsync(CreateGardenTask createGardenTask);
        Task UpdateGardenTaskAsync(int key, UpdateGardenTask updateGardenTask);
        Task DeleteGardenTaskAsync(int key);
    }
}
