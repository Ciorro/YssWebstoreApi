using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class VerifyAccountCommandHandler
        : ICommandHandler<VerifyAccountCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly TimeProvider _timeProvider;

        public VerifyAccountCommandHandler(IAccountRepository accountRepository, TimeProvider timeProvider)
        {
            _accountRepository = accountRepository;
            _timeProvider = timeProvider;
        }

        public async Task<Result> HandleAsync(VerifyAccountCommand message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAsync(message.AccountId);
            if (account is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            var currentTime = _timeProvider.GetUtcNow().UtcDateTime;

            if (account.Credentials.VerificationCode == message.VerificationCode &&
                account.Credentials.VerificationCodeExpiresAt > currentTime)
            {
                account.Credentials.IsVerified = true;
                account.Credentials.VerificationCode = null;
                account.Credentials.VerificationCodeExpiresAt = null;

                await _accountRepository.UpdateAsync(account);
                return Result.Ok();
            }

            return AccountErrors.InvalidVerificationCode;
        }
    }
}
