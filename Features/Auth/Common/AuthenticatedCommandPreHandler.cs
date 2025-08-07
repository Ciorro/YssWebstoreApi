using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Security;

namespace YssWebstoreApi.Features.Auth.Common
{
    public class AuthenticatedCommandPreHandler : ICommandPreHandler<AuthenticatedCommand>
    {
        private readonly IAccountRepository _accountRepository;

        public AuthenticatedCommandPreHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task PreHandleAsync(AuthenticatedCommand message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetByEmailAsync(message.Email);
            if (account is null)
            {
                return;
            }

            var correctPassword = new SaltedPassword
            {
                PasswordHash = account.Credentials.PasswordHash,
                PasswordSalt = account.Credentials.PasswordSalt
            };
            var incomingPassword = SaltedPassword.Create(
                password: message.Password,
                passwordSalt: correctPassword.PasswordSalt);

            if (incomingPassword != correctPassword)
            {
                return;
            }

            message.AuthenticatedAccount = account;
        }
    }
}
