using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.FruitHistories
{
    public class GetFruitHistory
    {
        public int HistoryId { get; set; }
        public string FruitName { get; set; } 
        public decimal Price { get; set; }
        public int UserId { get; set; }
        public string Location { get; set; } 
        public string Status { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
