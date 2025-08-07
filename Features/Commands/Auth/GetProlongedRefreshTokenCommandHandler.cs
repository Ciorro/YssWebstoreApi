using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;
using YssWebstoreApi.Security;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class GetProlongedRefreshTokenCommandHandler : IRequestHandler<GetProlongedRefreshTokenCommand, string?>
    {
        private readonly ICredentialsRepository _credentials;
        private readonly TimeProvider _timeProvider;
        private readonly IConfiguration _configuration;

        public GetProlongedRefreshTokenCommandHandler(ICredentialsRepository credentials, TimeProvider timeProvider, IConfiguration configuration)
        {
            _credentials = credentials;
            _timeProvider = timeProvider;
            _configuration = configuration;
        }

        public async Task<string?> Handle(GetProlongedRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var credentials = await _credentials.GetByAccountIdAsync(request.AccountId);
            if (credentials is not null)
            {
                var setsNewToken = string.IsNullOrEmpty(request.CurrentRefreshToken);
                if (setsNewToken)
                {
                    return await GenerateToken(credentials, request);
                }

                if (await ProlongToken(credentials, request))
                {
                    return credentials.RefreshToken;
                }
            }

            return null;
        }

        private async Task<string?> GenerateToken(Credentials credentials, GetProlongedRefreshTokenCommand request)
        {
            var currentTime = _timeProvider.GetUtcNow().UtcDateTime;
            var lifetime = _configuration.GetValue<TimeSpan?>("Security:RefreshTokenLifetime")
                ?? TimeSpan.FromDays(7);

            credentials.RefreshToken = SecurityUtils.GetRandomString(255);
            credentials.RefreshTokenExpiresAt = currentTime.Add(lifetime);

            if (await _credentials.UpdateAsync(credentials) == credentials.Id)
            {
                return credentials.RefreshToken;
            }

            return null;
        }

        private async Task<bool> ProlongToken(Credentials credentials, GetProlongedRefreshTokenCommand request)
        {
            var currentTime = _timeProvider.GetUtcNow().DateTime;
            var lifetime = _configuration.GetValue<TimeSpan?>("Security:RefreshTokenLifetime")
                ?? TimeSpan.FromDays(7);

            if (credentials.RefreshToken == request.CurrentRefreshToken &&
                currentTime <= credentials.RefreshTokenExpiresAt?.DateTime)
            {
                credentials.RefreshTokenExpiresAt = currentTime.Add(lifetime);
                return await _credentials.UpdateAsync(credentials) == credentials.Id;
            }

            return false;
        }
    }
}
