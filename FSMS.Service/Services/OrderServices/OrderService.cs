using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.FruitDiscountRepositories;
using FSMS.Entity.Repositories.FruitRepositories;
using FSMS.Entity.Repositories.OrderDetailRepositories;
using FSMS.Entity.Repositories.OrderRepositories;
using FSMS.Entity.Repositories.UserRepositories;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.Comments;
using FSMS.Service.ViewModels.OrderDetails;
using FSMS.Service.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private IOrderDetailRepository _orderDetailRepository;
        private IOrderRepository _orderRepository;
        private IUserRepository _userRepository;
        private IFruitRepository _fruitRepository;
        private IFruitDiscountRepository _fruitDiscountRepository;

        private IMapper _mapper;
        public OrderService(IOrderDetailRepository orderDetailRepository, IMapper mapper, IOrderRepository orderRepository, IUserRepository userRepository,
            IFruitRepository fruitRepository, IFruitDiscountRepository fruitDiscountRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _fruitRepository = fruitRepository;
            _fruitDiscountRepository = fruitDiscountRepository;
        }

        public async Task CreateOrderAsync(CreateOrder createOrder)
        {
            try
            {
                User existedUser = await _userRepository.GetByIDAsync(createOrder.UserId);
                if (existedUser == null)
                {
                    throw new Exception("User Id does not exist in the system.");
                }

                if (createOrder.OrderDetails == null || !createOrder.OrderDetails.Any())
                {
                    throw new Exception("OrderDetails list is empty or null.");
                }

                int lastOrderId = (await _orderRepository.GetAsync()).Max(x => x.OrderId);

                Order order = new Order()
                {
                    UserId = createOrder.UserId,
                    OrderDate = DateTime.Now,
                    DeliveryAddress = createOrder.DeliveryAddress,
                    PaymentMethod = createOrder.PaymentMethod,
                    PhoneNumber = createOrder.PhoneNumber,
                    Type = "",
                    Status = OrderEnum.Pending.ToString(),
                    CreatedDate = DateTime.Now,
                    OrderId = lastOrderId + 1,
                    ParentOrderId = createOrder.ParentOrderId
                };

                decimal totalAmount = 0;
                string orderType = "";

                foreach (var orderDetail in createOrder.OrderDetails)
                {
                    int fruitId = orderDetail.FruitId;
                    int fruitDiscountId = orderDetail.FruitDiscountId;
                    Fruit fruit = await _fruitRepository.GetByIDAsync(fruitId);
                    FruitDiscount fruitDiscount = await _fruitDiscountRepository.GetByIDAsync(fruitDiscountId);

                    decimal unitPrice = fruit.Price;

                    if (fruitDiscount != null && fruitDiscount.DiscountPercentage.HasValue)
                    {
                        decimal discountPercentage = fruitDiscount.DiscountPercentage.Value;
                        unitPrice = unitPrice * (1 - discountPercentage);
                    }

                    decimal orderDetailTotalAmount = orderDetail.Quantity * unitPrice;
                    totalAmount += orderDetailTotalAmount;

                    if (!string.IsNullOrWhiteSpace(fruit.OrderType))
                    {
                        orderType += fruit.OrderType + " ";
                    }

                    OrderDetail newOrderDetail = new OrderDetail()
                    {
                        FruitId = fruitId,
                        Quantity = orderDetail.Quantity,
                        UnitPrice = unitPrice,
                        TotalAmount = orderDetailTotalAmount,
                        FruitDiscountId = fruitDiscountId,
                        OderDetailType = fruit.OrderType,
                        CreatedDate = DateTime.Now,
                        Status = OrderEnum.Pending.ToString()
                    };

                    order.OrderDetails.Add(newOrderDetail);
                }

                order.Type = orderType.Trim();

                order.TotalAmount = totalAmount;

                await _orderRepository.InsertAsync(order);
                await _orderRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Inner Exception: " + ex.InnerException?.Message);
                throw new Exception(ex.Message);
            }
        }




        public async Task DeleteOrderAsync(int key)
        {
            try
            {

                Order existedOrder = await _orderRepository.GetByIDAsync(key);

                if (existedOrder == null)
                {
                    throw new Exception("Order ID does not exist in the system.");
                }

                existedOrder.Status = OrderEnum.Cancelled.ToString();

                await _orderRepository.UpdateAsync(existedOrder);
                await _orderRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GetOrder> GetAsync(int key)
        {
            try
            {
                Order order = await _orderRepository.GetByIDAsync(key);

                if (order == null)
                {
                    throw new Exception("Order ID does not exist in the system.");
                }

                List<GetOrder> orders = _mapper.Map<List<GetOrder>>(
                    await _orderRepository.GetAsync(includeProperties: "User")
                );

                GetOrder result = _mapper.Map<GetOrder>(order);

                IEnumerable<OrderDetail> orderDetails = await _orderDetailRepository.GetAsync(x => x.OrderId == key, includeProperties: "Fruit,FruitDiscount");

                if (orderDetails != null)
                {
                    result.OrderDetails = _mapper.Map<List<GetOrderDetail>>(orderDetails.ToList());
                }


                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetOrder>> GetAllAsync()
        {
            try
            {
                List<Order> orders = (await _orderRepository.GetAsync(includeProperties: "User")).ToList();

                // Map the list of orders to a list of GetOrder
                List<GetOrder> result = _mapper.Map<List<GetOrder>>(orders);

                // Now, you need to fetch and map order details for each order and add them to the result.
                foreach (var order in result)
                {
                    IEnumerable<OrderDetail> orderDetails = await _orderDetailRepository.GetAsync(x => x.OrderId == order.OrderId, includeProperties: "Fruit,FruitDiscount");

                    if (orderDetails != null)
                    {
                        order.OrderDetails = _mapper.Map<List<GetOrderDetail>>(orderDetails.ToList());
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task UpdateOrderAsync(int key, UpdateOrder updateOrder)
        {
            try
            {
                Order existedOrder = await _orderRepository.GetByIDAsync(key);

                if (existedOrder == null)
                {
                    throw new Exception("Order ID does not exist in the system.");
                }

                if (!string.IsNullOrEmpty(updateOrder.DeliveryAddress))
                {
                    existedOrder.DeliveryAddress = updateOrder.DeliveryAddress;
                }

                if (!string.IsNullOrEmpty(updateOrder.PaymentMethod))
                {
                    existedOrder.PaymentMethod = updateOrder.PaymentMethod;
                }
                if (!string.IsNullOrEmpty(updateOrder.PhoneNumber))
                {
                    existedOrder.PhoneNumber = updateOrder.PhoneNumber;
                }

                if (!string.IsNullOrEmpty(updateOrder.Status))
                {
                    if (updateOrder.Status != "Pending" && updateOrder.Status != "Cancelled")
                    {
                        throw new Exception("Status must be 'Pending' or 'Cancelled'.");
                    }
                    existedOrder.Status = updateOrder.Status;
                }
                existedOrder.UpdateDate = DateTime.Now;

                await _orderRepository.UpdateAsync(existedOrder);
                await _orderRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task ProcessOrderAsync(int orderId, string action)
        {
            try
            {
                IEnumerable<Order> orderList = await _orderRepository.GetAsync(
                    filter: o => o.OrderId == orderId,
                    includeProperties: "OrderDetails"
                );

                Order existedOrder = orderList.FirstOrDefault();

                if (existedOrder == null)
                {
                    throw new Exception("Order ID does not exist in the system.");
                }

                switch (action)
                {
                    case "Accepted":
                        if (existedOrder.Status == OrderEnum.Pending.ToString())
                        {
                            existedOrder.Status = OrderEnum.Accepted.ToString();
                            existedOrder.UpdateDate = DateTime.Now;
                            await _orderRepository.UpdateAsync(existedOrder);

                            foreach (var orderDetail in existedOrder.OrderDetails)
                            {
                                FruitDiscount fruitDiscount = await _fruitDiscountRepository.GetByIDAsync(orderDetail.FruitDiscountId);
                                if (fruitDiscount != null)
                                {
                                    fruitDiscount.DiscountThreshold -= (int)orderDetail.Quantity;
                                    await _fruitDiscountRepository.UpdateAsync(fruitDiscount);
                                }

                                Fruit fruit = await _fruitRepository.GetByIDAsync(orderDetail.FruitId);
                                if (fruit != null)
                                {
                                    fruit.QuantityAvailable -= orderDetail.Quantity;
                                    await _fruitRepository.UpdateAsync(fruit);
                                }
                                orderDetail.Status = OrderEnum.Accepted.ToString();
                                await _orderDetailRepository.UpdateAsync(orderDetail);
                            }

                            await _orderRepository.CommitAsync();
                            await _orderDetailRepository.CommitAsync();
                            await _fruitDiscountRepository.CommitAsync();
                            await _fruitRepository.CommitAsync();
                        }
                        else
                        {
                            throw new Exception("The order cannot be processed as its status is not 'Pending'.");
                        }
                        break;

                    case "Rejected":
                        if (existedOrder.Status == OrderEnum.Pending.ToString())
                        {
                            existedOrder.Status = OrderEnum.Rejected.ToString();
                            existedOrder.UpdateDate = DateTime.Now;
                            await _orderRepository.UpdateAsync(existedOrder);

                            foreach (var orderDetail in existedOrder.OrderDetails)
                            {
                                orderDetail.Status = OrderEnum.Rejected.ToString();
                                await _orderDetailRepository.UpdateAsync(orderDetail);
                            }

                            await _orderRepository.CommitAsync();
                            await _orderDetailRepository.CommitAsync();
                        }
                        else
                        {
                            throw new Exception("The order cannot be processed as its status is not 'Pending'.");
                        }
                        break;

                    default:
                        throw new Exception("Invalid action provided.");
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




    }
}
