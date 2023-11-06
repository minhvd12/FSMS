using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Utility;
using FSMS.Service.ViewModels.Authentications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using FSMS.Service.Services.FruitServices;
using FSMS.Service.ViewModels.Fruits;
using FSMS.Service.Validations.FruitFarmer;
using FSMS.Service.Validations.FruitSupplier;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/fruits")]
    [ApiController]
    public class FruitsController : ControllerBase
    {
        private IFruitService _fruitService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public FruitsController(IFruitService fruitService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _fruitService = fruitService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [Route("fruit-farmers")]
        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> GetAllFruitFarmers(string? fruitName = null, decimal? minPrice = null, decimal? maxPrice = null, bool activeOnly = false, DateTime? newestDate = null)
        {
            try
            {
                List<GetFruitFarmer> fruitFarmers = await _fruitService.GetAllFruitFarmerAsync(fruitName, minPrice, maxPrice, activeOnly, newestDate);
                return Ok(new
                {
                    Data = fruitFarmers
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




        [HttpGet("farmer/{id}")]
        [PermissionAuthorize("Farmer")]

        public async Task<IActionResult> GetFruitFarmerById(int id)
        {
            try
            {
                GetFruitFarmer fruitFarmer = await _fruitService.GetFruitFarmerAsync(id);
                return Ok(new
                {
                    Data = fruitFarmer
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


        [HttpPost("fruit-farmers")]
        [PermissionAuthorize("Farmer")]

        public async Task<IActionResult> CreateFruitFarmer([FromBody] CreateFruitFarmer createFruitFarmer)
        {
            var validator = new FruitFarmerValidator();
            var validationResult = validator.Validate(createFruitFarmer);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                await _fruitService.CreateFruitFarmerAsync(createFruitFarmer);

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


        [HttpPut("farmer/{id}")]
        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> UpdateFruitFarmer(int id, [FromBody] UpdateFruitFarmer updateFruitFarmer)
        {
            var validator = new UpdateFruitFarmerValidator();
            var validationResult = validator.Validate(updateFruitFarmer);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _fruitService.UpdateFruitFarmerAsync(id, updateFruitFarmer);
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

        [HttpDelete("farmer/{id}")]
        [PermissionAuthorize("Farmer")]
        public async Task<IActionResult> DeleteFruitFarmer(int id)
        {
            try
            {
                await _fruitService.DeleteFruitFarmerAsync(id);
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

        [HttpGet]
        [Route("fruit-suppliers")]
        public async Task<IActionResult> GetAllFruitSuppliers(string? fruitName = null, decimal? minPrice = null, decimal? maxPrice = null, bool activeOnly = false, DateTime? newestDate = null)
        {
            try
            {
                List<GetFruitSupplier> fruitSuppliers = await _fruitService.GetAllFruitSupplierAsync(fruitName, minPrice, maxPrice, activeOnly, newestDate);
                return Ok(new
                {
                    Data = fruitSuppliers
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




        [HttpGet("supplier/{id}")]
        public async Task<IActionResult> GetFruitSupplierById(int id)
        {
            try
            {
                GetFruitSupplier fruitSupplier = await _fruitService.GetFruitSupplierAsync(id);
                return Ok(new
                {
                    Data = fruitSupplier
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


        [HttpPost("fruit-suppliers")]
        [PermissionAuthorize("Supplier")]

        public async Task<IActionResult> CreateFruitSupplier([FromBody] CreateFruitSupplier createFruitSupplier)
        {
            var validator = new FruitSupplierValidator();
            var validationResult = validator.Validate(createFruitSupplier);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                await _fruitService.CreateFruitSupplierAsync(createFruitSupplier);

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


        [HttpPut("supplier/{id}")]
        [PermissionAuthorize("Supplier")]
        public async Task<IActionResult> UpdateFruitSupplier(int id, [FromBody] UpdateFruitSupplier updateFruitSupplier)
        {
            var validator = new UpdateFruitSupplierValidator();
            var validationResult = validator.Validate(updateFruitSupplier);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _fruitService.UpdateFruitSupplierAsync(id, updateFruitSupplier);
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

        [HttpDelete("supplier/{id}")]
        [PermissionAuthorize("Supplier")]
        public async Task<IActionResult> DeleteFruitSupplier(int id)
        {
            try
            {
                await _fruitService.DeleteFruitSupplierAsync(id);
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
