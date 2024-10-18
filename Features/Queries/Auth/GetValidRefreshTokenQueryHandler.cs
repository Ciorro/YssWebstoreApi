using MediatR;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Queries.Auth
{
    public class GetValidRefreshTokenQueryHandler : IRequestHandler<GetValidRefreshTokenQuery, string?>
    {
        private readonly ICredentialsRepository _credentials;
        private readonly TimeProvider _timeProvider;

        public GetValidRefreshTokenQueryHandler(ICredentialsRepository credentials, TimeProvider timeProvider)
        {
            _credentials = credentials;
            _timeProvider = timeProvider;
        }

        public async Task<string?> Handle(GetValidRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var credentials = await _credentials.GetByAccountIdAsync(request.AccountId);
            if (credentials?.RefreshTokenExpiresAt > _timeProvider.GetUtcNow())
            {
                return credentials.RefreshToken;
            }

            return null;
        }
    }
}
