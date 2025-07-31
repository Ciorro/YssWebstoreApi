using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class UploadAvatarCommandHandler
        : ICommandHandler<UploadAvatarCommand, Result>
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IAvatarStorage _avatarStorage;
        private readonly TimeProvider _timeProvider;

        public UploadAvatarCommandHandler(IRepository<Account> accountRepository, IAvatarStorage avatarStorage, TimeProvider timeProvider)
        {
            _accountRepository = accountRepository;
            _avatarStorage = avatarStorage;
            _timeProvider = timeProvider;
        }

        public async Task<Result> HandleAsync(UploadAvatarCommand message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAsync(message.AccountId);
            if (account is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            string? fileName = await _avatarStorage.UploadAvatar(
                message.AccountId, message.File);

            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;
            var id = Guid.CreateVersion7(creationTime);

            account.Avatar = new Resource
            {
                Id = id,
                CreatedAt = creationTime,
                UpdatedAt = creationTime,
                Path = fileName
            };

            await _accountRepository.UpdateAsync(account);
            return Result.Ok();
        }
    }
}
