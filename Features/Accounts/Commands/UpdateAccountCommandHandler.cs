using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    internal class UpdateAccountCommandHandler
        : ICommandHandler<UpdateAccountCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;

        public UpdateAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Result> HandleAsync(UpdateAccountCommand message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAsync(message.AccountId);
            if (account is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            account.DisplayName = message.DisplayName;
            account.StatusText = message.StatusText;
            await _accountRepository.UpdateAsync(account);

            return Result.Ok();
        }
    }
}
