using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.ViewModel
{
    public class CreateFileAttachment
    {
        public Guid? ProfileId { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public IFormFile File { get; set; }
    }
    
}
