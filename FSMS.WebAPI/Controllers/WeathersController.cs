using FSMS.Service.Services.FruitHistoryServices;
using FSMS.Service.Services.WeatherServices;
using FSMS.Service.Utility;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.FruitHistories;
using FSMS.Service.ViewModels.Weather;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/weathers")]
    [ApiController]
    public class WeathersController : ControllerBase
    {
        private IWeatherService _weatherService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public WeathersController(IWeatherService weatherService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _weatherService = weatherService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [PermissionAuthorize("Expert", "Farmer")]

        public async Task<IActionResult> GetAllWeathers()
        {
            try
            {
                List<GetWeather> weathers = await _weatherService.GetAllAsync();
                return Ok(new
                {
                    Data = weathers
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("scraping-weather-area")]
        [PermissionAuthorize("Expert")]

        public async Task<IActionResult> CreateWeatherAreaFromNCHMF(int userId)
        {
            try
            {
                await _weatherService.ScrapeWeatherAreaAndSaveToDatabaseAsync(userId);

                return Ok("Data scraped and saved to the database successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("scraping-city-weather")]
        [PermissionAuthorize("Expert")]
        public async Task<IActionResult> CreateCityWeatherFromNCHMF(int userId)
        {
            try
            {
                await _weatherService.ScrapeCityWeatherAndSaveToDatabaseAsync(userId);

                return Ok("Data scraped and saved to the database successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
