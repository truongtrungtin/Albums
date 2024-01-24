using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data.ViewModel
{
    public class ProfileViewModel
    {
        public Guid ProfileId { get; set; }
        public int ProfileCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? LastEditTime { get; set; }
        public Guid? LastEditBy { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public bool? Actived { get; set; }
    }
}