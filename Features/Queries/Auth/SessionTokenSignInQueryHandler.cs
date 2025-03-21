using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Queries.Auth
{
    public class SessionTokenSignInQueryHandler : IRequestHandler<SessionTokenSignInQuery, Account?>
    {
        private readonly TimeProvider _timeProvider;
        private readonly ISessionRepository _sessions;
        private readonly IRepository<Account> _accounts;

        public SessionTokenSignInQueryHandler(ISessionRepository sessions, IRepository<Account> accounts, TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            _sessions = sessions;
            _accounts = accounts;
        }

        public async Task<Account?> Handle(SessionTokenSignInQuery request, CancellationToken cancellationToken)
        {
            var session = await _sessions.GetSessionByToken(request.SessionToken);
            if (session is null || session.AccountId != request.AccountId)
            {
                return null;
            }

            var currentTime = _timeProvider.GetUtcNow().DateTime;
            if (currentTime > session.ExpiresAt?.DateTime)
            {
                return null;
            }

            return await _accounts.GetAsync(request.AccountId);
        }
    }
}
