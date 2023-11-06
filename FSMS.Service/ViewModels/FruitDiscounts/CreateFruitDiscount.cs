using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.FruitDiscounts
{
    public class CreateFruitDiscount
    {
        public int FruitId { get; set; }
        public string DiscountName { get; set; }
        public int DiscountThreshold { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime DiscountExpiryDate { get; set; }
        public decimal DepositAmount { get; set; }

    }
}
