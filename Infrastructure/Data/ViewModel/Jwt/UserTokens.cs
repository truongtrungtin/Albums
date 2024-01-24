using System;
using System.Globalization;

namespace Infrastructure.Data.ViewModel
{
    public class UserTokens
    {
        public Guid ProfileId { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public int? ProfileCode { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public TimeSpan? Validaty { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
