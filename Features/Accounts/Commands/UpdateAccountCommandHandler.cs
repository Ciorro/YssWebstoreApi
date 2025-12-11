using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    internal class UpdateAccountCommandHandler
        : ICommandHandler<UpdateAccountCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly TimeProvider _timeProvider;

        public UpdateAccountCommandHandler(IAccountRepository accountRepository, TimeProvider timeProvider)
        {
            _accountRepository = accountRepository;
            _timeProvider = timeProvider;
        }

        public async Task<Result> HandleAsync(UpdateAccountCommand message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAsync(message.AccountId);
            if (account is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            account.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
            account.DisplayName = message.DisplayName;
            account.StatusText = message.StatusText;
            await _accountRepository.UpdateAsync(account);

            return Result.Ok();
        }
    }
}
