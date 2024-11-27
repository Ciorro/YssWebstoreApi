using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class VerifyAccountCommandHandler : IRequestHandler<VerifyAccountCommand, bool>
    {
        private readonly IDbConnection _cn;
        private readonly ICredentialsRepository _credentials;
        private readonly TimeProvider _timeProvider;

        public VerifyAccountCommandHandler(IDbConnection cn, ICredentialsRepository credentials, TimeProvider timeProvider)
        {
            _cn = cn;
            _credentials = credentials;
            _timeProvider = timeProvider;
        }

        public async Task<bool> Handle(VerifyAccountCommand request, CancellationToken cancellationToken)
        {
            var credentials = await _credentials.GetByAccountIdAsync(request.AccountId);
            if (credentials is not null)
            {
                var currentTime = _timeProvider.GetUtcNow().DateTime;

                if (credentials.VerificationCode == request.VerificationCode &&
                    credentials.VerificationCodeExpiresAt?.DateTime >= currentTime)
                {
                    credentials.IsVerified = true;
                    credentials.VerificationCode = null;
                    credentials.VerificationCodeExpiresAt = null;

                    return await _credentials.UpdateAsync(credentials) == credentials.Id;
                }
            }

            return false;
        }
    }
}
