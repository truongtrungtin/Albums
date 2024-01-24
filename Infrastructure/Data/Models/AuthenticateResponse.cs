using Infrastructure.Data.ViewModel;
using System;


namespace Infrastructure.Data.Models
{
    public class AuthenticateResponse
    {
       
        public AuthenticateResponse(AccountViewModel user, string token)
        {
            Id = user.AccountId;
            ProfileCode = user.ProfileCode;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            Token = token;
        }

        public Guid Id { get; set; }
        public int ProfileCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

    }

}