using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Entities.Identity;
using System.Text;
using Core.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Data.ViewModel;

namespace Infrastructure.Services
{
    public class TokenGenerationService : ITokenGenerationService
    {
        private readonly JwtSettings _tokenSettings;

        public TokenGenerationService(IOptions<JwtSettings> tokenSettings)
        {
            _tokenSettings = tokenSettings.Value;
        }

        public string GenerateToken(IEnumerable<Claim> claims, int expiryInMinutes = 60)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.IssuerSigningKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _tokenSettings.ValidIssuer,
                audience: _tokenSettings.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiryInMinutes),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}

