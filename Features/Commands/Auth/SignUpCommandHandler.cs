using MediatR;
using System.Data;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;
using YssWebstoreApi.Security;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, ulong?>
    {
        private readonly IDbConnection _connection;
        private readonly IRepository<Account> _accounts;
        private readonly IRepository<Credentials> _credentials;

        public SignUpCommandHandler(IDbConnection connection, IRepository<Account> accounts, IRepository<Credentials> credentials)
        {
            _connection = connection;
            _accounts = accounts;
            _credentials = credentials;
        }

        public async Task<ulong?> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            using var transaction = _connection.BeginTransaction();

            try
            {
                var createdAccountId = await _accounts.CreateAsync(new Account
                {
                    UniqueName = request.UniqueName,
                    DisplayName = request.DisplayName
                });

                var password = new SaltedPassword(request.Password);
                var createdCredentialsId = await _credentials.CreateAsync(new Credentials
                {
                    AccountId = createdAccountId,
                    Email = request.Email,
                    PasswordHash = password.PasswordHash,
                    PasswordSalt = password.PasswordSalt
                });
                
                transaction.Commit();
                return createdAccountId;
            }
            catch
            {
                transaction.Rollback();
                return null;
            }
        }
    }
}
