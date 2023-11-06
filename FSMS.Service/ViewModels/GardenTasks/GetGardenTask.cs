using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.GardenTasks
{
    public class GetGardenTask
    {
        public int GardenTaskId { get; set; }
        public string GardenTaskName { get; set; } 
        public string Description { get; set; } 
        public DateTime GardenTaskDate { get; set; }
        public string Status { get; set; }
        public string GardenName { get; set; }
        public string PlantName { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
