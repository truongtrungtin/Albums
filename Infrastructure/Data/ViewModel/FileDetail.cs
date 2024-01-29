using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.ViewModel
{
    public class FileDetails
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public string Content { get; set; } // Add this property if you want to include file content

        // Additional properties can be added based on your requirements
    }

}
