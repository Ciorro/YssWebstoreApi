using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class VerifyAccountCommandHandler : IRequestHandler<VerifyAccountCommand, bool>
    {
        private readonly IRepository<Account> _accounts;
        private readonly ICredentialsRepository _credentials;
        private readonly TimeProvider _timeProvider;

        public VerifyAccountCommandHandler(IRepository<Account> accounts, ICredentialsRepository credentials, TimeProvider timeProvider)
        {
            _accounts = accounts;
            _credentials = credentials;
            _timeProvider = timeProvider;
        }

        public async Task<bool> Handle(VerifyAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accounts.GetAsync(request.AccountId);
            var credentials = await _credentials.GetByAccountIdAsync(request.AccountId);

            if (credentials is not null && account is not null)
            {
                var currentTime = _timeProvider.GetUtcNow().DateTime;

                if (credentials.VerificationCode == request.VerificationCode &&
                    credentials.VerificationCodeExpiresAt?.DateTime >= currentTime)
                {
                    account.IsVerified = true;
                    credentials.VerificationCode = null;
                    credentials.VerificationCodeExpiresAt = null;
                    
                    await _accounts.UpdateAsync(account);
                    await _credentials.UpdateAsync(credentials);

                    return true;
                }
            }

            return false;
        }
    }
}
