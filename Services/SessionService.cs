using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YssWebstoreApi.Api.DTO.Auth;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Security;

namespace YssWebstoreApi.Services
{
    public class SessionService : ISessionService
    {
        private readonly IConfiguration _configuration;
        private readonly TimeProvider _timeProvider;

        public SessionService(IConfiguration configuration, TimeProvider timeProvider)
        {
            _configuration = configuration;
            _timeProvider = timeProvider;
        }

        public TokenCredentials CreateSession(Account account, string? deviceInfo)
        {
            DateTime creationTime = _timeProvider.GetUtcNow().UtcDateTime;

            var session = new Session
            {
                Id = Guid.CreateVersion7(),
                CreatedAt = creationTime,
                UpdatedAt = creationTime,
                SessionToken = SecurityUtils.GetRandomString(255),
                DeviceInfo = deviceInfo
            };
            account.Sessions.Add(session);

            return new TokenCredentials()
            {
                AccessToken = CreateSessionToken(account),
                SessionToken = session.SessionToken
            };
        }

        public TokenCredentials? UpdateSession(Account account, Session session)
        {
            var currentTime = _timeProvider.GetUtcNow().UtcDateTime;
            var sessionLifetime = _configuration.GetValue<TimeSpan?>("Security:SessionTokenLifetime")
                ?? TimeSpan.FromDays(7);

            if (currentTime.Subtract(session.UpdatedAt) < sessionLifetime)
            {
                session.UpdatedAt = currentTime;
                return new TokenCredentials()
                {
                    AccessToken = CreateSessionToken(account),
                    SessionToken = session.SessionToken
                };
            }

            return null;
        }

        private string CreateSessionToken(Account account)
        {
            var currentTime = _timeProvider.GetUtcNow().UtcDateTime;
            var lifetime = _configuration.GetValue<TimeSpan?>("Security:AccessTokenLifetime")
                ?? TimeSpan.FromMinutes(1);

            var token = new JwtSecurityToken(
                claims: [
                    new Claim("accountId", account.Id.ToString()),
                    new Claim("uniqueName", account.UniqueName),
                    new Claim("isVerified", account.Credentials.IsVerified.ToString())
                ],
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
