using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YssWebstoreApi.Models;
using YssWebstoreApi.Models.DTOs.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Security;

namespace YssWebstoreApi.Services
{
    public class SessionServiceOld : ISessionServiceOld
    {
        private readonly IAccountRepository _accounts;
        private readonly IConfiguration _configuration;
        private readonly TimeProvider _time;

        public SessionServiceOld(IAccountRepository accounts, IConfiguration configuration, TimeProvider time)
        {
            _accounts = accounts;
            _configuration = configuration;
            _time = time;
        }

        public async Task<TokenCredentials> CreateSession(Guid accountId, string? deviceInfo = "")
        {
            var creationTime = _time.GetUtcNow().UtcDateTime;

            var account = await _accounts.GetAsync(accountId);
            if (account is null)
            {
                throw new InvalidOperationException($"Account with ID '{accountId}' not found.");
            }

            var session = new Session
            {
                Id = Guid.CreateVersion7(),
                CreatedAt = creationTime,
                UpdatedAt = creationTime,
                SessionToken = SecurityUtils.GetRandomString(255),
                DeviceInfo = deviceInfo
            };

            account.Sessions.Add(session);
            await _accounts.UpdateAsync(account);

            return new TokenCredentials
            {
                AccessToken = CreateSessionToken(account),
                SessionToken = session.SessionToken
            };
        }

        public async Task<TokenCredentials> UpdateSession(Guid accountId, string sessionToken)
        {
            var account = await _accounts.GetAsync(accountId);
            if (account is null)
            {
                throw new InvalidOperationException($"Account with ID '{accountId}' not found.");
            }

            var session = account.Sessions.FirstOrDefault(x => x.SessionToken == sessionToken);
            if (session is null)
            {
                throw new InvalidOperationException("Invalid session.");
            }

            var currentTime = _time.GetUtcNow().UtcDateTime;
            var sessionLifetime = _configuration.GetValue<TimeSpan?>("Security:SessionTokenLifetime")
                ?? TimeSpan.FromDays(7);

            if (currentTime.Subtract(session.UpdatedAt) >= sessionLifetime)
            {
                throw new InvalidOperationException("Session expired.");
            }

            session.UpdatedAt = currentTime;
            await _accounts.UpdateAsync(account);

            return new TokenCredentials
            {
                AccessToken = CreateSessionToken(account),
                SessionToken = session.SessionToken
            };
        }

        public async Task<bool> DeleteSession(Guid accountId, string sessionToken)
        {
            var account = await _accounts.GetAsync(accountId);
            if (account is null)
            {
                return false;
            }

            var session = account.Sessions.FirstOrDefault(x =>
                x.SessionToken == sessionToken);
            account.Sessions.Remove(session!);

            await _accounts.UpdateAsync(account);
            return true;
        }

        public async Task<bool> DeleteAllSessions(Guid accountId)
        {
            var account = await _accounts.GetAsync(accountId);
            if (account is null)
            {
                return false;
            }

            account.Sessions.Clear();

            await _accounts.UpdateAsync(account);
            return true;
        }

        private string CreateSessionToken(Account account)
        {
            var currentTime = _time.GetUtcNow().UtcDateTime;
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
