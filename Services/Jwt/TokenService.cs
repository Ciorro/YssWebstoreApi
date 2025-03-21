using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YssWebstoreApi.Models;

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

        public string GetJwt(Account account)
        {
            ArgumentNullException.ThrowIfNull(account.Id, nameof(Account.Id));

            var currentTime = _timeProvider.GetUtcNow().UtcDateTime;
            var lifetime = _configuration.GetValue<TimeSpan?>("Security:AccessTokenLifetime")
                ?? TimeSpan.FromMinutes(1);

            Claim[] claims = [
                new Claim("account_id", account.Id.Value.ToString()),
                new Claim("is_verified", account.IsVerified.ToString())
            ];

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: currentTime,
                expires: currentTime.Add(lifetime),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration.GetSection("Security:JwtKey").Value!)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
