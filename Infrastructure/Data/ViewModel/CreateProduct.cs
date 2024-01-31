using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.ViewModel
{
    public class CreateFile
    {
        public string Name { get; set; }
        public Guid? ProfileId { get; set; }
        public LocationViewModel? LocationImage { get; set; }
        public IFormFile File { get; set; }
    }
    
}
