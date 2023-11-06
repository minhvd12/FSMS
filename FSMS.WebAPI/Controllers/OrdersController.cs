using FSMS.Service.Services.OrderServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.Validations;
using FSMS.Service.Validations.Order;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IOrderService _orderService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public OrdersController(IOrderService orderService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _orderService = orderService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [PermissionAuthorize("Customer", "Supplier", "Farmer")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                List<GetOrder> orders = await _orderService.GetAllAsync();
                return Ok(new
                {
                    Data = orders
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
        [PermissionAuthorize("Customer", "Supplier", "Farmer")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                GetOrder order = await _orderService.GetAsync(id);
                return Ok(new
                {
                    Data = order
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
        [PermissionAuthorize("Customer", "Supplier")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder createOrder)
        {
            var validator = new OrderValidator();
            var validationResult = validator.Validate(createOrder);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }

                await _orderService.CreateOrderAsync(createOrder);

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
        [PermissionAuthorize("Customer", "Supplier")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrder updateOrder)
        {
            var validator = new UpdateOrderValidator();
            var validationResult = validator.Validate(updateOrder);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult);
                }
                await _orderService.UpdateOrderAsync(id, updateOrder);
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
        [PermissionAuthorize("Customer", "Supplier")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
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

        [HttpPost]
        [Route("{id}/process")]
        [PermissionAuthorize("Supplier", "Farmer")]
        public async Task<IActionResult> ProcessOrder(int id, string action)
        {
            try
            {
                if (action != "Accepted" && action != "Rejected")
                {
                    return BadRequest(new
                    {
                        Message = "Invalid action. Please specify 'Accepted' or 'Rejected' in the URL."
                    });
                }

                await _orderService.ProcessOrderAsync(id, action);
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
