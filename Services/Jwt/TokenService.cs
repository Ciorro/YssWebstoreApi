using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace YssWebstoreApi.Services.Jwt
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly TimeProvider _timeProvider;

        public TokenService(IConfiguration configuration, TimeProvider timeProvider)
        {
            _configuration = configuration;
            _timeProvider = timeProvider;
        }

        public string GetJwt(params Claim[] claims)
        {
            var now = _timeProvider.GetUtcNow().UtcDateTime;

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
