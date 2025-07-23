using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Sessions.Commands
{
    public class DeleteSessionCommandHandler
        : ICommandHandler<DeleteSessionCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;

        public DeleteSessionCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Result> HandleAsync(DeleteSessionCommand message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAsync(message.AccountId);
            if (account is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            var session = account.Sessions.FirstOrDefault(x =>
                x.SessionToken == message.SessionToken);
            account.Sessions.Remove(session!);

            await _accountRepository.UpdateAsync(account);

            return Result.Ok();
        }
    }
}
