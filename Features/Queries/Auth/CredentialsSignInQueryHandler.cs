using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;
using YssWebstoreApi.Security;

namespace YssWebstoreApi.Features.Queries.Auth
{
    public class CredentialsSignInQueryHandler : IRequestHandler<CredentialsSignInQuery, Account?>
    {
        private readonly IRepository<Account> _accounts;
        private readonly ICredentialsRepository _credentials;

        public CredentialsSignInQueryHandler(ICredentialsRepository credentials, IRepository<Account> accounts)
        {
            _accounts = accounts;
            _credentials = credentials;
        }

        public async Task<Account?> Handle(CredentialsSignInQuery request, CancellationToken cancellationToken)
        {
            var credentials = await _credentials.GetByEmailAsync(request.Email);
            if (credentials is null)
            {
                return null;
            }

            var correctPassword = new SaltedPassword
            {
                PasswordHash = credentials.PasswordHash!,
                PasswordSalt = credentials.PasswordSalt!
            };
            var incomingPassword = SaltedPassword.Create(request.Password, correctPassword.PasswordSalt);

            if (incomingPassword != correctPassword)
            {
                return null;
            }

            return await _accounts.GetAsync(credentials.AccountId!.Value);
        }
    }
}
