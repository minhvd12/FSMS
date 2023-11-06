using FSMS.Service.ViewModels.FruitHistories;
using FSMS.Service.ViewModels.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.WeatherServices
{
    public interface IWeatherService
    {
        Task<List<GetWeather>> GetAllAsync();
        Task ScrapeWeatherAreaAndSaveToDatabaseAsync(int userId);
        Task ScrapeCityWeatherAndSaveToDatabaseAsync(int userId);
    }
}
