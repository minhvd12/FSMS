using FSMS.Entity.Models;
using FSMS.Service.ViewModels.Orders;
using FSMS.Service.ViewModels.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.PaymentServices
{
    public interface IPaymentService
    {
        Task<List<GetPayment>> GetAllAsync();
        Task<GetPayment> GetAsync(int key);
        Task<PaymentWithOrderDetails> CreatePaymentAsync(CreatePayment createPayment);
        Task DeletePaymentAsync(int key);
        Task<PaymentWithOrderDetails> GetByOrderIdAsync(int orderId);
        Task ProcessPayment(int paymentId, ProcessPaymentRequest processPaymentRequest);



    }
}
