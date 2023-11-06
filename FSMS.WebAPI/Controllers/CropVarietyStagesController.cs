using FSMS.Service.Services.CropVarietyGrowthTaskServices;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Utility;
using FSMS.Service.Validations.CropVarietyGrowthTask;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.CropVarietyGrowthTasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using FSMS.Service.Services.CropVarietyStageServices;
using FSMS.Service.ViewModels.CropVarietyStages;
using FSMS.Service.Validations.CropVarietyStage;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/crop-variety-stages")]
    [ApiController]
    public class CropVarietyStagesController : ControllerBase
    {
        private ICropVarietyStageService _cropVarietyStageService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public CropVarietyStagesController(ICropVarietyStageService cropVarietyStageService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _cropVarietyStageService = cropVarietyStageService;
            _jwtAuthOptions = jwtAuthOptions;
        }
        [HttpGet]
        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> GetAllCropVarietyStages(string? stageName = null, DateTime? startDate = null, bool activeOnly = false)
        {
            try
            {
                List<GetCropVarietyStage> cropVarietyStages = await _cropVarietyStageService.GetAllAsync(stageName, startDate, activeOnly);
                return Ok(new
                {
                    Data = cropVarietyStages
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


        [HttpGet("{id}")]
        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> GetCropVarietyStageById(int id)
        {
            try
            {
                GetCropVarietyStage cropVarietyStage = await _cropVarietyStageService.GetAsync(id);
                return Ok(new
                {
                    Data = cropVarietyStage
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
        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> CreateCropVarietyStage([FromBody] CreateCropVarietyStage createCropVarietyStage)
        {
            var validator = new CropVarietyStageValidator();
            var validationResult = validator.Validate(createCropVarietyStage);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                await _cropVarietyStageService.CreateCropVarietyStageAsync(createCropVarietyStage);

                return Ok();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
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


        [HttpPut("{id}")]
        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> UpdateCropVarietyStage(int id, [FromBody] UpdateCropVarietyStage updateCropVarietyStage)
        {
            var validator = new UpdateCropVarietyStageValidator();
            var validationResult = validator.Validate(updateCropVarietyStage);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _cropVarietyStageService.UpdateCropVarietyStageAsync(id, updateCropVarietyStage);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> DeleteCropVarietyStage(int id)
        {
            try
            {
                await _cropVarietyStageService.DeleteCropVarietyStageAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
    }
}
