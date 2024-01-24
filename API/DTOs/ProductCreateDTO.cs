using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace API.DTOs
{
    public class ProductCreateDTO
    {
        public Guid Id
        {
            get
            {
                return Guid.NewGuid();
            }
        }
        public string? Name { get; set; }
        public string?Description { get; set; }
        public string? PictureUrl { get; set; }
        public string? Location { get; set; }
        public string? ProductType { get; set; }
        public double? Size { get; set; }

        public TimeSpan? DuringTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UploadTime { get; set; }

        public Guid? ProductTypeId { get; set; }

        public List<FileContentResult> files { get; set;}
    }
}
