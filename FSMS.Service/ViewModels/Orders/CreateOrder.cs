using FSMS.Service.ViewModels.OrderDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Orders
{
    public class CreateOrder
    {
        public int UserId { get; set; }
        public int ParentOrderId { get; set; }
        public DateTime OrderDate { get; set; }   
        public string DeliveryAddress { get; set; } 
        public string PaymentMethod { get; set; } 
        public string PhoneNumber { get; set; }
        public List<CreateOrderDetail> OrderDetails { get; set; }

    }
}
