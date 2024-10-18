using Dapper;
using MediatR;
using System.Data;
using System.Security.Claims;
using YssWebstoreApi.Models.DTOs.Auth;
using YssWebstoreApi.Repositories.Abstractions;
using YssWebstoreApi.Security;
using YssWebstoreApi.Services.Jwt;

namespace YssWebstoreApi.Features.Queries.Auth
{
    public class SignInQueryHandler : IRequestHandler<SignInQuery, (ulong accountId, string token)?>
    {
        private readonly ITokenService _tokenService;
        private readonly ICredentialsRepository _credentials;

        public SignInQueryHandler(ICredentialsRepository credentials, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _credentials = credentials;
        }

        public async Task<(ulong accountId, string token)?> Handle(SignInQuery request, CancellationToken cancellationToken)
        {
            var credentials = await _credentials.GetByEmailAsync(request.Email);
            if (credentials is null)
            {
                return null;
            }

            var correctPassword = new SaltedPassword
            {
                PasswordHash = credentials.PasswordHash!,
                PasswordSalt = credentials.PasswordSalt!
            };
            var incomingPassword = SaltedPassword.Create(request.Password, correctPassword.PasswordSalt);

            if (incomingPassword != correctPassword)
            {
                return null;
            }

            var accessToken = _tokenService.GetJwt([
                new Claim("account_id", credentials.AccountId.ToString()!),
                new Claim("is_verified", credentials.IsVerified.ToString())
            ]);

            return (credentials.AccountId!.Value, accessToken);
        }
    }
}
