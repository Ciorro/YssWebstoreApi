using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YssWebstoreApi.Mappers;
using YssWebstoreApi.Middlewares.Attributes;
using YssWebstoreApi.Models;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Auth;
using YssWebstoreApi.Repositories.Abstractions;
using YssWebstoreApi.Security;
using YssWebstoreApi.Services.Jwt;

namespace YssWebstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICredentialRepository _credentialsRepository;
        private readonly ITokenService _tokenService;
        private readonly TimeProvider _timeProvider;

        public AuthController(ICredentialRepository credentialsRepository, ITokenService tokenService, TimeProvider timeProvider)
        {
            _credentialsRepository = credentialsRepository;
            _tokenService = tokenService;
            _timeProvider = timeProvider;
        }

        [HttpPost("signin"), AllowUnverified]
        public async Task<IActionResult> SignIn([FromBody] SignInCredentials signInCredentials)
        {
            var credentials = await _credentialsRepository.GetByEmailAsync(signInCredentials.Email);
            if (credentials is null)
            {
                return Unauthorized();
            }

            var correctPassword = new SaltedPassword()
            {
                PasswordHash = credentials.PasswordHash!,
                PasswordSalt = credentials.PasswordSalt!
            };
            var incomingPassword = new SaltedPassword(signInCredentials.Password, credentials.PasswordSalt!);

            if (correctPassword != incomingPassword)
            {
                return Unauthorized();
            }

            var accessToken = _tokenService.GetJwt([
                new Claim("account_id", credentials.AccountId.ToString()!),
                new Claim("is_verified", credentials.IsVerified.ToString())
            ]);
            var refreshToken = (await SetRefreshToken(credentials, signInCredentials.RememberMe))!.Token;

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(
            [FromServices] IAccountRepository accountRepository, [FromBody] CreateAccount createAccount)
        {
            var password = new SaltedPassword(createAccount.Credentials.Password);

            if (await accountRepository.CreateAsync(createAccount.ToAccount()))
            {
                var createdAccount = await accountRepository.GetByUniqueNameAsync(createAccount.UniqueName);

                var credentials = new Credentials
                {
                    AccountId = createdAccount!.Id,
                    Email = createAccount.Credentials.Email,
                    PasswordHash = password.PasswordHash,
                    PasswordSalt = password.PasswordSalt
                };

                if (await _credentialsRepository.CreateAsync(credentials))
                {
                    credentials.Id = (await _credentialsRepository.GetByAccountAsync(credentials.AccountId!.Value))?.Id;

                    var accessToken = _tokenService.GetJwt([
                        new Claim("account_id", credentials.AccountId.ToString()!),
                        new Claim("is_verified", credentials.IsVerified.ToString())
                    ]);
                    var refreshToken = (await SetRefreshToken(credentials, false))!.Token;

                    return Ok(new
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    });
                }
            }

            return BadRequest();
        }

        [HttpPost("{id:int}/refresh")]
        public async Task<IActionResult> Refresh([FromRoute] uint id, [FromBody] string? refreshToken)
        {
            var credentials = await _credentialsRepository.GetByAccountAsync(id);
            if (credentials is null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(refreshToken) && !Request.Cookies.TryGetValue("refresh-token", out refreshToken))
            {
                return Unauthorized();
            }

            var tokenExpiresIn = credentials.RefreshTokenExpiresAt?.Subtract(_timeProvider.GetUtcNow());

            if (refreshToken != credentials.RefreshToken || tokenExpiresIn <= TimeSpan.Zero)
            {
                return Unauthorized();
            }

            if (tokenExpiresIn < TimeSpan.FromDays(1))
            {
                refreshToken = (await SetRefreshToken(credentials, false))!.Token;
            }

            var accessToken = _tokenService.GetJwt([
                new Claim("account_id", credentials.AccountId.ToString()!),
                new Claim("is_verified", credentials.IsVerified.ToString())
            ]);

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        private async Task<RefreshToken?> SetRefreshToken(Credentials credentials, bool disposable)
        {
            var token = SecurityUtils.GetRandomString(255);
            var expires = _timeProvider.GetUtcNow().AddDays(7);

            credentials.RefreshToken = token;
            credentials.RefreshTokenExpiresAt = expires;

            if (await _credentialsRepository.UpdateAsync(credentials.Id!.Value, credentials))
            {
                Response.Cookies.Append("refresh-token", token, new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    Expires = disposable ? null : expires,
                    SameSite = SameSiteMode.None,
                    Secure = true
                });

                return new RefreshToken(token, expires);
            }

            return null;
        }
    }
}
