using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.ViewModel;
using Core.Entities;
using Infrastructure.Data.Library;
using Infrastructure.Identity;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Core.Interfaces;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services
{
    public interface IUserService
    {
        //IEnumerable<ApplicationUser> GetAll();
        Task<AccountViewModel> GetById(Guid id);
        //ProfileViewModel GetByCode(int Code);
        //void Register(RegisterRequest model, out string ErrorMessages);
        //void Edit(int ProfileCode, ApplicationUser viewModel);

        //void ChangePassword(int ProfileCode, string newPassword);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications

        private JwtSettings _appSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationIdentityDbContext _dbContext;

        public UserService(
            UserManager<ApplicationUser> userManager, JwtSettings appSettings)
        {
            _appSettings = appSettings;

            _userManager = userManager;
            _dbContext = new ApplicationIdentityDbContext();
        }
        public async Task<AccountViewModel> GetById(Guid id)
        {

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return null;
            }
            var userDTO = new AccountViewModel
            {
                AccountId = Guid.Parse(user.Id),
                Username = user.Email,
                FirstName = user.DisplayName,
                LastName = user.DisplayName,
                isSysadmin = false
            };
            return userDTO;
        }

    }
}