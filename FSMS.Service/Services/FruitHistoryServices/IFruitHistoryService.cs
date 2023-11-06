using FSMS.Service.ViewModels.CropVariety;
using FSMS.Service.ViewModels.FruitHistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.FruitHistoryServices
{
    public interface IFruitHistoryService
    {
        Task<List<GetFruitHistory>> GetAllAsync();
        Task ScrapeFarmerMarketAndSaveToDatabaseAsync(int userId);
        Task ScrapeThucPhamNhanhAndSaveToDatabaseAsync(int userId);


    }
}
