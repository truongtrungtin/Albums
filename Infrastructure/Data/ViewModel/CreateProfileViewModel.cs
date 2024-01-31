using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data.ViewModel
{
    public class CreateProfileViewModel
    {
        public string ProfileName { get; set; }
        public IFormFile Avatar { get; set; }
    }
}