using MediatR;
using System.Security.Claims;
using YssWebstoreApi.Models.DTOs.Auth;
using YssWebstoreApi.Repositories.Abstractions;
using YssWebstoreApi.Services.Jwt;

namespace YssWebstoreApi.Features.Queries.Auth
{
    public class TokenSignInQueryHandler : IRequestHandler<TokenSignInQuery, TokenCredentials?>
    {
        private readonly ITokenService _tokenService;
        private readonly TimeProvider _timeProvider;
        private readonly ICredentialsRepository _credentials;

        public TokenSignInQueryHandler(ICredentialsRepository credentials, ITokenService tokenService, TimeProvider timeProvider)
        {
            _tokenService = tokenService;
            _timeProvider = timeProvider;
            _credentials = credentials;
        }

        public async Task<TokenCredentials?> Handle(TokenSignInQuery request, CancellationToken cancellationToken)
        {
            var credentials = await _credentials.GetByAccountIdAsync(request.AccountId);
            if (credentials is null)
            {
                return null;
            }

            if (credentials.RefreshToken != request.RefreshToken ||
                credentials.RefreshTokenExpiresAt < _timeProvider.GetUtcNow())
            {
                return null;
            }

            var accessToken = _tokenService.GetJwt([
                new Claim("account_id", credentials.AccountId.ToString()!),
                new Claim("is_verified", credentials.IsVerified.ToString())
            ]);

            return new TokenCredentials(accessToken, credentials.RefreshToken);
        }
    }
}
