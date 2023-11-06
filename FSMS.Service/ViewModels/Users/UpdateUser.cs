using FSMS.Service.ViewModels.Files;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Users
{
    public class UpdateUser : FileViewModel
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        /*public string ProfileImageUrl { get; set; }*/
        public IFormFile? ImageMomoUrl { get; set; } = null;
    }
}
