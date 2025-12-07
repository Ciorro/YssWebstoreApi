using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class UploadAvatarCommandHandler
        : ICommandHandler<UploadAvatarCommand, ValueResult<string>>
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IAvatarStorage _avatarStorage;

        public UploadAvatarCommandHandler(IRepository<Account> accountRepository, IAvatarStorage avatarStorage)
        {
            _accountRepository = accountRepository;
            _avatarStorage = avatarStorage;
        }

        public async Task<ValueResult<string>> HandleAsync(UploadAvatarCommand message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAsync(message.AccountId);
            if (account is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            Resource avatarResource = await _avatarStorage.UploadAvatar(
                message.AccountId, message.File);
            account.Avatar = avatarResource;

            await _accountRepository.UpdateAsync(account);
            return avatarResource.PublicUrl!;
        }
    }
}
