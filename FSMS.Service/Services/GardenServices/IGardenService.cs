using FSMS.Service.ViewModels.CropVariety;
using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.GardenServices
{
    public interface IGardenService
    {
        Task<List<GetGarden>> GetAllAsync(string? gardenName = null, bool activeOnly = false);
        Task<GetGarden> GetAsync(int key);
        Task CreateGardenAsync(CreateGarden createGarden);
        Task UpdateGardenAsync(int key, UpdateGarden updateGarden);
        Task DeleteGardenAsync(int key);
    }
}
