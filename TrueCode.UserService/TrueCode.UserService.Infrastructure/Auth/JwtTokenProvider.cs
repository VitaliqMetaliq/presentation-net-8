using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrueCode.Shared.Contracts.Entities;
using TrueCode.UserService.Application;
using TrueCode.UserService.Application.Abstractions;

namespace TrueCode.UserService.Infrastructure.Auth
{
    internal class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly SigningCredentials _signingCredentials;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _tokenLLifetimeMinutes;

        public JwtTokenProvider(string issuer, string audience, int tokenLifeTimeMinutes, string secret)
        {
            _issuer = issuer;
            _audience = audience;
            _tokenLLifetimeMinutes = tokenLifeTimeMinutes;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        public string GenerateJwtToken(UserEntity user)
        {
            var claims = new List<Claim>
            {
                new(ApplicationConstants.Auth.UserIdClaim, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Name, user.Name),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_tokenLLifetimeMinutes),
                signingCredentials: _signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
