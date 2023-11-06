using FSMS.Service.ViewModels.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Posts
{
    public class UpdatePost : FileViewModel
    {
      /*  [Required(ErrorMessage = "PostTitle is required.")]
        [MaxLength(4000, ErrorMessage = "PostTitle must be less than or equals 4000 characters.")]*/
        public string PostTitle { get; set; }

       /* [Required(ErrorMessage = "PostContent is required.")]
        [MaxLength(4000, ErrorMessage = "PostContent must be less than or equals 4000 characters.")]*/
        public string PostContent { get; set; }

       /* [Url(ErrorMessage = "Invalid URL format for Profile Image.")]*/
        /*public string PostImage { get; set; }*/

       /* [MaxLength(255, ErrorMessage = "Type must be less than or equals 255 characters.")]*/
        public string Type { get; set; }

       /* [RegularExpression("^(Active|InActive)$", ErrorMessage = "Status must be 'Active' or 'InActive'.")]*/
        public string Status { get; set; }

    }
}
