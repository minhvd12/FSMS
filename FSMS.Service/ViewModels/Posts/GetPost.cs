using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.ViewModels.Posts
{
    public class GetPost
    {
        public int PostId { get; set; }
        public string FullName { get; set; }
        public string PostTitle { get; set; } 
        public string PostContent { get; set; } 
        public string PostImage { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
