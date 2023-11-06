using FSMS.Service.Utility.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Plants
{
    public class UpdatePlant
    {
        public string PlantName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public DateTime PlantingDate { get; set; }

        public DateTime HarvestingDate { get; set; }

        public double QuantityPlanted { get; set; }


        public double EstimatedHarvestQuantity { get; set; }

        public string Status { get; set; }

    }
}
