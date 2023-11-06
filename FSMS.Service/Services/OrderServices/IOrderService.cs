using FSMS.Entity.Models;
using FSMS.Service.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.OrderServices
{
    public interface IOrderService
    {
        Task<List<GetOrder>> GetAllAsync();
        Task<GetOrder> GetAsync(int key);
        Task CreateOrderAsync(CreateOrder createOrder);
        Task UpdateOrderAsync(int key, UpdateOrder updateOrder);
        Task DeleteOrderAsync(int key);
        Task ProcessOrderAsync(int orderId, string action);

    }
}
