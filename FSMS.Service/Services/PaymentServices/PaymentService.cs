using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.FruitRepositories;
using FSMS.Entity.Repositories.NotificationRepositories;
using FSMS.Entity.Repositories.OrderDetailRepositories;
using FSMS.Entity.Repositories.OrderRepositories;
using FSMS.Entity.Repositories.PaymentRepositories;
using FSMS.Entity.Repositories.UserRepositories;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.Notifications;
using FSMS.Service.ViewModels.OrderDetails;
using FSMS.Service.ViewModels.Orders;
using FSMS.Service.ViewModels.Payments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private IPaymentRepository _paymentRepository;
        private IOrderRepository _orderRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private IUserRepository _userRepository;
        private IFruitRepository _fruitRepository;
        private IMapper _mapper;
        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, IOrderRepository orderRepository, IUserRepository userRepository, IFruitRepository fruitRepository, IOrderDetailRepository orderDetailRepository)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _fruitRepository = fruitRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<PaymentWithOrderDetails> CreatePaymentAsync(CreatePayment createPayment)
        {
            try
            {
                User existedUser = await _userRepository.GetByIDAsync(createPayment.UserId);
                if (existedUser == null)
                {
                    throw new Exception("User ID does not exist in the system.");
                }

                decimal amountToPay = 0;
                string paymentType = "";

                Order existedOrder = await _orderRepository.GetByIDAsync(createPayment.OrderId);
                if (existedOrder == null)
                {
                    throw new Exception("Order ID does not exist in the system.");
                }
                paymentType = existedOrder.Type;
                amountToPay = existedOrder.TotalAmount;
                int lastId = (await _paymentRepository.GetAsync()).Max(x => x.PaymentId);
                Payment payment = new Payment()
                {
                    OrderId = createPayment.OrderId,
                    PaymentDate = DateTime.Now,
                    PaymentType = paymentType,
                    PaymentMethod = createPayment.PaymentMethod,
                    UserId = createPayment.UserId,
                    Amount = amountToPay,
                    Status = PaymentEnum.Pending.ToString(),
                    CreatedDate = DateTime.Now,
                    PaymentId = lastId + 1
                };
                await _paymentRepository.InsertAsync(payment);
                await _paymentRepository.CommitAsync();

                // Retrieve order details
                IEnumerable<OrderDetail> orderDetailEnumerable = await _orderDetailRepository.GetAsync(od => od.OrderId == createPayment.OrderId, includeProperties: "Fruit,FruitDiscount");
                List<OrderDetail> orderDetails = orderDetailEnumerable.ToList();

                PaymentWithOrderDetails paymentWithOrderDetails = new PaymentWithOrderDetails()
                {
                    Payment = payment,
                    OrderDetails = _mapper.Map<List<GetOrderDetail>>(orderDetails)
                };

                return paymentWithOrderDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeletePaymentAsync(int key)
        {
            try
            {
                Payment existedPayment = await _paymentRepository.GetByIDAsync(key);

                if (existedPayment == null)
                {
                    throw new Exception("Payment ID does not exist in the system.");
                }

                existedPayment.Status = PaymentEnum.Cancelled.ToString();

                await _paymentRepository.UpdateAsync(existedPayment);
                await _paymentRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetPayment> GetAsync(int key)
        {
            try
            {
                Payment payment = await _paymentRepository.GetByIDAsync(key);

                if (payment == null)
                {
                    throw new Exception("Payment ID does not exist in the system.");
                }

                List<GetPayment> payments = _mapper.Map<List<GetPayment>>(
                    await _paymentRepository.GetAsync(includeProperties: "User")
                );

                // Lấy thông tin chi tiết đơn hàng (Order Details) cho Payment
                IEnumerable<OrderDetail> orderDetailEnumerable = await _orderDetailRepository.GetAsync(od => od.OrderId == payment.OrderId, includeProperties: "Fruit,FruitDiscount");
                List<GetOrderDetail> orderDetails = _mapper.Map<List<GetOrderDetail>>(orderDetailEnumerable);

                // Khai báo biến để lưu thông tin người bán sản phẩm
                string sellerImageMomoUrl = null;

                foreach (var getOrderDetail in orderDetails)
                {
                    // Lấy thông tin sản phẩm (Product)
                    Fruit fruit = await _fruitRepository.GetByIDAsync(getOrderDetail.FruitId);

                    if (fruit == null)
                    {
                        throw new Exception("Fruit does not exist in the system.");
                    }

                    // Lấy thông tin người bán sản phẩm (User)
                    User seller = await _userRepository.GetByIDAsync(fruit.UserId);

                    if (seller == null)
                    {
                        throw new Exception("Seller (User) does not exist in the system.");
                    }

                    // Gán giá trị cho sellerImageMomoUrl
                    sellerImageMomoUrl = seller.ImageMomoUrl;
                }

                GetPayment result = _mapper.Map<GetPayment>(payment);
                result.OrderDetails = orderDetails;
                result.SellerImageMomoUrl = sellerImageMomoUrl;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<GetPayment>> GetAllAsync()
        {
            try
            {
                // Lấy tất cả thanh toán (Payments) từ repository
                IEnumerable<Payment> paymentsEnumerable = await _paymentRepository.GetAsync(includeProperties: "User");

                List<GetPayment> payments = new List<GetPayment>();

                foreach (var payment in paymentsEnumerable)
                {
                    // Lấy thông tin chi tiết đơn hàng (Order Details) cho mỗi Payment
                    IEnumerable<OrderDetail> orderDetailEnumerable = await _orderDetailRepository.GetAsync(od => od.OrderId == payment.OrderId, includeProperties: "Fruit,FruitDiscount");
                    List<GetOrderDetail> orderDetails = _mapper.Map<List<GetOrderDetail>>(orderDetailEnumerable);

                    // Khai báo biến để lưu thông tin của tất cả người bán
                    string sellerImageMomoUrl = null;

                    foreach (var getOrderDetail in orderDetails)
                    {
                        // Lấy thông tin sản phẩm (Product)
                        Fruit fruit = await _fruitRepository.GetByIDAsync(getOrderDetail.FruitId);

                        if (fruit == null)
                        {
                            throw new Exception("Fruit does not exist in the system.");
                        }

                        // Lấy thông tin người bán sản phẩm (User)
                        User seller = await _userRepository.GetByIDAsync(fruit.UserId);

                        if (seller == null)
                        {
                            throw new Exception("Seller (User) does not exist in the system.");
                        }

                        // Gán giá trị cho sellerImageMomoUrl
                        sellerImageMomoUrl = seller.ImageMomoUrl;

                    }

                    // Tạo một đối tượng GetPayment và đặt OrderDetails và SellerImageMomoUrl
                    GetPayment paymentWithOrderDetails = _mapper.Map<GetPayment>(payment);
                    paymentWithOrderDetails.OrderDetails = orderDetails;
                    paymentWithOrderDetails.SellerImageMomoUrl = sellerImageMomoUrl;

                    payments.Add(paymentWithOrderDetails);
                }

                return payments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





        public async Task<PaymentWithOrderDetails> GetByOrderIdAsync(int orderId)
        {
            try
            {
                Payment payment = await _paymentRepository.GetByIDAsync(orderId);

                if (payment == null)
                {
                    throw new Exception("Payment with Order ID does not exist in the system.");
                }

                // Lấy thông tin chi tiết đơn hàng (Order Details) cho Payment
                IEnumerable<OrderDetail> orderDetailEnumerable = await _orderDetailRepository.GetAsync(od => od.OrderId == orderId);
                List<OrderDetail> orderDetails = orderDetailEnumerable.ToList();

                PaymentWithOrderDetails paymentWithOrderDetails = new PaymentWithOrderDetails()
                {
                    Payment = payment,
                    OrderDetails = _mapper.Map<List<GetOrderDetail>>(orderDetails)
                };

                return paymentWithOrderDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ProcessPayment(int paymentId, ProcessPaymentRequest processPaymentRequest)
        {
            try
            {
                Payment existedPayment = await _paymentRepository.GetByIDAsync(paymentId);

                if (existedPayment == null)
                {
                    throw new Exception("Payment does not exist for the given Payment ID.");
                }

                if (!string.IsNullOrEmpty(processPaymentRequest.Status))
                {
                    if (processPaymentRequest.Status != "Completed" && processPaymentRequest.Status != "Failed" && processPaymentRequest.Status != "Refunded")
                    {
                        throw new Exception("Status must be 'Completed' or 'Failed' or 'Refunded'.");
                    }

                    existedPayment.Status = processPaymentRequest.Status;
                    existedPayment.UpdateDate = DateTime.Now;

                    await _paymentRepository.UpdateAsync(existedPayment);
                    await _paymentRepository.CommitAsync();
                }
                else
                {
                    throw new Exception("Status is required.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }






    }
}
