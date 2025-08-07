using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class DeleteAvatarCommandHandler
        : ICommandHandler<DeleteAvatarCommand, Result>
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IImageStorage _imageStorage;

        public DeleteAvatarCommandHandler(IRepository<Account> accountRepository, IImageStorage imageStorage)
        {
            _accountRepository = accountRepository;
            _imageStorage = imageStorage;
        }

        public async Task<Result> HandleAsync(DeleteAvatarCommand message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAsync(message.AccountId);
            if (account is null || account.Avatar is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            await _imageStorage.Delete(account.Avatar.Path);
            account.Avatar = null;
            await _accountRepository.UpdateAsync(account);
            return Result.Ok();
        }
    }
}
