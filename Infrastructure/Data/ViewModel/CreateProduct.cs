using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.ViewModel
{
    public class CreateProduct
    {
        public string Name { get; set; }
        public string ProductType { get; set; }
        public LocationViewModel? LocationImage { get; set; }
        public FileDetails File { get; set; }
    }
    
}
