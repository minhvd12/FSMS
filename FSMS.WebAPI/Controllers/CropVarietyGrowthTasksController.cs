using FSMS.Service.Services.GardenTaskServices;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Utility;
using FSMS.Service.Validations.GardenTask;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.GardenTasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using FSMS.Service.Services.CropVarietyGrowthTaskServices;
using FSMS.Service.ViewModels.CropVarietyGrowthTasks;
using FluentValidation;
using FSMS.Service.Validations.CropVarietyGrowthTask;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/crop-variety-growth-tasks")]
    [ApiController]
    public class CropVarietyGrowthTasksController : ControllerBase
    {
        private ICropVarietyGrowthTaskService _cropVarietyGrowthTaskService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public CropVarietyGrowthTasksController(ICropVarietyGrowthTaskService cropVarietyGrowthTaskService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _cropVarietyGrowthTaskService = cropVarietyGrowthTaskService;
            _jwtAuthOptions = jwtAuthOptions;
        }
        [HttpGet]
        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> GetAllCropVarietyGrowthTasks(string? taskName = null, DateTime? startDate = null, bool activeOnly = false)
        {
            try
            {
                List<GetCropVarietyGrowthTask> cropVarietyGrowthTasks = await _cropVarietyGrowthTaskService.GetAllAsync(taskName, startDate, activeOnly);
                return Ok(new
                {
                    Data = cropVarietyGrowthTasks
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
        public async Task<IActionResult> GetCropVarietyGrowthTaskById(int id)
        {
            try
            {
                GetCropVarietyGrowthTask cropVarietyGrowthTask = await _cropVarietyGrowthTaskService.GetAsync(id);
                return Ok(new
                {
                    Data = cropVarietyGrowthTask
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
        public async Task<IActionResult> CreateCropVarietyGrowthTask([FromBody] CreateCropVarietyGrowthTask createCropVarietyGrowthTask)
        {
            var validator = new CropVarietyGrowthTaskValidator();
            var validationResult = validator.Validate(createCropVarietyGrowthTask);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                await _cropVarietyGrowthTaskService.CreateCropVarietyGrowthTaskAsync(createCropVarietyGrowthTask);

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
        public async Task<IActionResult> UpdateCropVarietyGrowthTask(int id, [FromBody] UpdateCropVarietyGrowthTask updateCropVarietyGrowthTask)
        {
            var validator = new UpdateCropVarietyGrowthTaskValidator();
            var validationResult = validator.Validate(updateCropVarietyGrowthTask);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _cropVarietyGrowthTaskService.UpdateCropVarietyGrowthTaskAsync(id, updateCropVarietyGrowthTask);
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
        public async Task<IActionResult> DeleteCropVarietyGrowthTask(int id)
        {
            try
            {
                await _cropVarietyGrowthTaskService.DeleteCropVarietyGrowthTaskAsync(id);
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
