using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class ExtendSessionCommandHandler : IRequestHandler<ExtendSessionCommand, Session?>
    {
        private readonly ISessionRepository _sessions;
        private readonly IConfiguration _configuration;
        private readonly TimeProvider _timeProvider;

        public ExtendSessionCommandHandler(ISessionRepository sessions, IConfiguration configuration, TimeProvider timeProvider)
        {
            _sessions = sessions;
            _configuration = configuration;
            _timeProvider = timeProvider;
        }

        public async Task<Session?> Handle(ExtendSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await _sessions.GetSessionByToken(request.SessionToken);
            if (session is null)
            {
                return null;
            }

            if (session.AccountId == request.Account.Id)
            {
                var currentTime = _timeProvider.GetUtcNow().UtcDateTime;
                var sessionLifetime = _configuration.GetValue<TimeSpan?>("Security:SessionTokenLifetime")
                    ?? TimeSpan.FromDays(7);

                session.ExpiresAt = currentTime.Add(sessionLifetime);
                if ((await _sessions.UpdateAsync(session)).HasValue)
                {
                    return session;
                }
            }

            return null;
        }
    }
}
