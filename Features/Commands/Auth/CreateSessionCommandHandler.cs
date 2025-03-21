using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;
using YssWebstoreApi.Security;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, Session?>
    {
        private readonly IRepository<Session> _sessions;
        private readonly IConfiguration _configuration;
        private readonly TimeProvider _timeProvider;

        public CreateSessionCommandHandler(IRepository<Session> sessions, IConfiguration configuration, TimeProvider timeProvider)
        {
            _sessions = sessions;
            _configuration = configuration;
            _timeProvider = timeProvider;
        }

        public async Task<Session?> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            var currentTime = _timeProvider.GetUtcNow().UtcDateTime;
            var sessionLifetime = _configuration.GetValue<TimeSpan?>("Security:SessionTokenLifetime")
                ?? TimeSpan.FromDays(7);

            var session = new Session()
            {
                AccountId = request.Account.Id,
                SessionToken = SecurityUtils.GetRandomString(255),
                ExpiresAt = currentTime.Add(sessionLifetime)
            };

            var sessionId = await _sessions.CreateAsync(session);
            if (sessionId.HasValue)
            {
                return await _sessions.GetAsync(sessionId.Value);
            }

            return null;
        }
    }
}
