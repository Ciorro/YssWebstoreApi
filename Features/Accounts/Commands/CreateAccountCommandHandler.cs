using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Models;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Security;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class CreateAccountCommandHandler
        : ICommandHandler<CreateAccountCommand, Result<Guid>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly TimeProvider _timeProvider;

        public CreateAccountCommandHandler(IAccountRepository accountRepository, TimeProvider timeProvider)
        {
            _accountRepository = accountRepository;
            _timeProvider = timeProvider;
        }

        public async Task<Result<Guid>> HandleAsync(CreateAccountCommand message, CancellationToken cancellationToken = default)
        {
            var creationDate = _timeProvider.GetUtcNow().UtcDateTime;
            var saltedPassword = new SaltedPassword(message.Password);

            var account = new Account
            {
                Id = Guid.CreateVersion7(),
                CreatedAt = creationDate,
                UpdatedAt = creationDate,
                UniqueName = message.UniqueName,
                DisplayName = message.DisplayName,
                Credentials = new Credentials
                {
                    Id = Guid.CreateVersion7(),
                    CreatedAt = creationDate,
                    UpdatedAt = creationDate,
                    Email = message.Email,
                    PasswordHash = saltedPassword.PasswordHash,
                    PasswordSalt = saltedPassword.PasswordSalt
                }
            };

            await _accountRepository.InsertAsync(account);

            return account.Id;
        }
    }
}
