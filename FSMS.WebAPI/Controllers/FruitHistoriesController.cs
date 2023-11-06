using FSMS.Service.Services.FruitHistoryServices;
using FSMS.Service.Services.UserServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Validations;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.FruitHistories;
using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/fruit-histories")]
    [ApiController]
    public class FruitHistoriesController : ControllerBase
    {
        private IFruitHistoryService _fruitHistoryService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public FruitHistoriesController(IFruitHistoryService fruitHistoryService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _fruitHistoryService = fruitHistoryService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [PermissionAuthorize("Expert", "Farmer")]

        public async Task<IActionResult> GetAllFruitHistories()
        {
            try
            {
                List<GetFruitHistory> fruitHistory = await _fruitHistoryService.GetAllAsync();
                return Ok(new
                {
                    Data = fruitHistory
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
        [Route("scraping-farmer-market")]
        [PermissionAuthorize("Expert")]

        public async Task<IActionResult> CreateFruitHistoriesFromFarmerMarket(int userId)
        {
            try
            {
                await _fruitHistoryService.ScrapeFarmerMarketAndSaveToDatabaseAsync(userId);

                return Ok("Data scraped and saved to the database successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost]
        [Route("scraping-thuc-pham-nhanh")]
        [PermissionAuthorize("Expert")]

        public async Task<IActionResult> CreateFruitHistoriesFromThucPhamNhanh(int userId)
        {
            try
            {
                await _fruitHistoryService.ScrapeThucPhamNhanhAndSaveToDatabaseAsync(userId);

                return Ok("Data scraped and saved to the database successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


    }
}
