using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Features.Sessions.Commands
{
    public class DeleteAllSessionsCommandHandler
        : ICommandHandler<DeleteAllSessionsCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;

        public DeleteAllSessionsCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Result> HandleAsync(DeleteAllSessionsCommand message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAsync(message.AccountId);
            if (account is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            account.Sessions.Clear();
            await _accountRepository.UpdateAsync(account);
            return Result.Ok();
        }
    }
}
