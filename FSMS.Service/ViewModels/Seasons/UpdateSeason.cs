﻿using FSMS.Service.Utility.ValidationAttributes;
using FSMS.Service.ViewModels.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Seasons
{
    public class UpdateSeason : FileViewModel
    {
       /* [Required(ErrorMessage = "Season Name is required.")]
        [MaxLength(200, ErrorMessage = "Season Name must be less than or equals 200 characters.")]*/
        public string SeasonName { get; set; }

        public string Description { get; set; }
        /*public string Image { get; set; }*/

       /* [Required(ErrorMessage = "Start date is required.")]*/
        public DateTime StartDate { get; set; }

       /* [Required(ErrorMessage = "End Date is required.")]*/
        public DateTime EndDate { get; set; }

        public string Status { get; set; }


    }
}
