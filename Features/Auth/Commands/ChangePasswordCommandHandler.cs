using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Security;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Auth.Commands
{
    public class ChangePasswordCommandHandler
        : ICommandHandler<ChangePasswordCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly TimeProvider _timeProvider;

        public ChangePasswordCommandHandler(IAccountRepository accountRepository, TimeProvider timeProvider)
        {
            _accountRepository = accountRepository;
            _timeProvider = timeProvider;
        }

        public async Task<Result> HandleAsync(ChangePasswordCommand message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAsync(message.AccountId);
            if (account is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            var correctPassword = new SaltedPassword
            {
                PasswordHash = account.Credentials.PasswordHash,
                PasswordSalt = account.Credentials.PasswordSalt
            };
            var incomingPassword = SaltedPassword.Create(
                message.OldPassword, 
                account.Credentials.PasswordSalt);

            if (correctPassword != incomingPassword)
            {
                return AuthErrors.BadCredentials;
            }

            var newPassword = new SaltedPassword(message.NewPassword);
            account.Credentials.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
            account.Credentials.PasswordHash = newPassword.PasswordHash;
            account.Credentials.PasswordSalt = newPassword.PasswordSalt;
            await _accountRepository.UpdateAsync(account);

            return Result.Ok();
        }
    }
}
