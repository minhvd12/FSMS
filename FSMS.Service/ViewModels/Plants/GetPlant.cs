using FSMS.Service.ViewModels.FruitImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Plants
{
    public class GetPlant
    {
        public int PlantId { get; set; }
        public string PlantName { get; set; } 
        public string Description { get; set; } 
        public DateTime PlantingDate { get; set; }
        public DateTime HarvestingDate { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }
        public int GardenId { get; set; }
        public string GardenName { get; set; }
        public string CropVarietyName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public double QuantityPlanted { get; set; }
        public double EstimatedHarvestQuantity { get; set; }
        public string md5Hash { get; set; }



    }
}
