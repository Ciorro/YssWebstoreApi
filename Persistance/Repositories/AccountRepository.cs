using Dapper;
using System.Data;
using YssWebstoreApi.Models;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Persistance.Repositories
{
    public class AccountRepository : IRepository<Account>
    {
        private readonly IDbConnection _db;

        public AccountRepository(IDbConnection dbConnection)
        {
            _db = dbConnection;
            _db.Open();
        }

        public async Task<Account?> GetAsync(Guid id)
        {
            Account? result = null;

            await _db.QueryAsync<Account, Credentials, Session, Account>(
                """
                SELECT
                	Accounts.Id,
                	Accounts.CreatedAt,
                	Accounts.UpdatedAt,
                	Accounts.UniqueName,
                	Accounts.DisplayName,
                	Accounts.StatusText,
                	Credentials.Id,
                	Credentials.CreatedAt,
                	Credentials.UpdatedAt,
                	Credentials.Email,
                	Credentials.PasswordHash,
                	Credentials.PasswordSalt,
                	Credentials.VerificationCode,
                	Credentials.VerificationCodeExpiresAt,
                	Credentials.PasswordResetCode,
                	Credentials.PasswordResetCodeExpiresAt,
                	Credentials.IsVerified,
                	Sessions.Id,
                	Sessions.CreatedAt,
                	Sessions.UpdatedAt,
                	Sessions.SessionToken,
                	Sessions.DeviceInfo
                FROM
                	Accounts
                	INNER JOIN Credentials ON Credentials.AccountId = Accounts.Id
                	LEFT JOIN Sessions ON Sessions.AccountId = Accounts.Id
                WHERE
                	Accounts.Id = @Id
                """,
                (account, credentials, session) =>
                {
                    result ??= account;
                    result.Credentials = credentials;

                    if (session is not null)
                    {
                        result.Sessions.Add(session);
                    }

                    return result;
                },
                new { Id = id });

            return result;
        }

        public async Task InsertAsync(Account entity)
        {
            using var transaction = _db.BeginTransaction();

            await _db.ExecuteAsync(
                $"""
                INSERT INTO Accounts (
                    Id, 
                    CreatedAt, 
                    UpdatedAt, 
                    UniqueName, 
                    DisplayName,
                    StatusText
                ) VALUES (
                    @{nameof(Account.Id)},
                    @{nameof(Account.CreatedAt)},
                    @{nameof(Account.UpdatedAt)},
                    @{nameof(Account.UniqueName)},
                    @{nameof(Account.DisplayName)},
                    @{nameof(Account.StatusText)}
                );
                """,
                new
                {
                    entity.Id,
                    entity.CreatedAt,
                    entity.UpdatedAt,
                    entity.UniqueName,
                    entity.DisplayName,
                    entity.StatusText
                }, transaction);

            await _db.ExecuteAsync(
                $"""
                INSERT INTO Credentials (
                    AccountId,
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    Email,
                    PasswordHash,
                    PasswordSalt,
                    VerificationCode,
                    VerificationCodeExpiresAt,
                    PasswordResetCode,
                    PasswordResetCodeExpiresAt,
                    IsVerified
                ) VALUES (
                    @AccountId,
                    @{nameof(Credentials.Id)},
                    @{nameof(Credentials.CreatedAt)},
                    @{nameof(Credentials.UpdatedAt)},
                    @{nameof(Credentials.Email)},
                    @{nameof(Credentials.PasswordHash)},
                    @{nameof(Credentials.PasswordSalt)},
                    @{nameof(Credentials.VerificationCode)},
                    @{nameof(Credentials.VerificationCodeExpiresAt)},
                    @{nameof(Credentials.PasswordResetCode)},
                    @{nameof(Credentials.PasswordResetCodeExpiresAt)},
                    @{nameof(Credentials.IsVerified)}
                );
                """,
                new
                {
                    AccountId = entity.Id,
                    entity.Credentials.Id,
                    entity.Credentials.CreatedAt,
                    entity.Credentials.UpdatedAt,
                    entity.Credentials.Email,
                    entity.Credentials.PasswordHash,
                    entity.Credentials.PasswordSalt,
                    entity.Credentials.VerificationCode,
                    entity.Credentials.VerificationCodeExpiresAt,
                    entity.Credentials.PasswordResetCode,
                    entity.Credentials.PasswordResetCodeExpiresAt,
                    entity.Credentials.IsVerified
                }, transaction);

            await _db.ExecuteAsync(
                $"""
                INSERT INTO Sessions (
                    AccountId,
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    SessionToken,
                    DeviceInfo
                ) VALUES (
                    @AccountId,
                    @{nameof(Session.Id)},
                    @{nameof(Session.CreatedAt)},
                    @{nameof(Session.UpdatedAt)},
                    @{nameof(Session.SessionToken)},
                    @{nameof(Session.DeviceInfo)}                
                );
                """,
                entity.Sessions.Select(x => new
                {
                    AccountId = entity.Id,
                    x.Id,
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.SessionToken,
                    x.DeviceInfo
                }), transaction);

            transaction.Commit();
        }

        public async Task UpdateAsync(Account entity)
        {
            using var transaction = _db.BeginTransaction();

            await _db.ExecuteAsync(
                $"""
                UPDATE Accounts 
                SET Id = @{nameof(Account.Id)}, 
                    CreatedAt = @{nameof(Account.CreatedAt)}, 
                    UpdatedAt = @{nameof(Account.UpdatedAt)}, 
                    UniqueName = @{nameof(Account.UniqueName)}, 
                    DisplayName = @{nameof(Account.DisplayName)},
                    StatusText = @{nameof(Account.StatusText)}
                WHERE
                    Id = @{nameof(Account.Id)}
                """,
                entity, transaction);

            await _db.ExecuteAsync(
                $"""
                UPDATE Credentials
                SET Id = @{nameof(Credentials.Id)},
                    CreatedAt = @{nameof(Credentials.CreatedAt)},
                    UpdatedAt = @{nameof(Credentials.UpdatedAt)},
                    Email = @{nameof(Credentials.Email)},
                    PasswordHash = @{nameof(Credentials.PasswordHash)},
                    PasswordSalt = @{nameof(Credentials.PasswordSalt)},
                    VerificationCode = @{nameof(Credentials.VerificationCode)},
                    VerificationCodeExpiresAt = @{nameof(Credentials.VerificationCodeExpiresAt)},
                    PasswordResetCode = @{nameof(Credentials.PasswordResetCode)},
                    PasswordResetCodeExpiresAt = @{nameof(Credentials.PasswordResetCodeExpiresAt)},
                    IsVerified = @{nameof(Credentials.IsVerified)}
                WHERE
                    Id = @{nameof(Credentials.Id)}
                """,
                entity.Credentials, transaction);

            await _db.ExecuteAsync(
                $"""
                INSERT INTO Sessions (
                    AccountId,
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    SessionToken,
                    DeviceInfo
                ) VALUES (
                    @AccountId,
                    @{nameof(Session.Id)},
                    @{nameof(Session.CreatedAt)},
                    @{nameof(Session.UpdatedAt)},
                    @{nameof(Session.SessionToken)},
                    @{nameof(Session.DeviceInfo)} 
                ) ON CONFLICT (Id) DO UPDATE
                SET Id = @{nameof(Session.Id)},
                    CreatedAt = @{nameof(Session.CreatedAt)},
                    UpdatedAt = @{nameof(Session.UpdatedAt)},
                    SessionToken = @{nameof(Session.SessionToken)},
                    DeviceInfo = @{nameof(Session.DeviceInfo)}
                """,
                entity.Sessions.Select(x => new
                {
                    AccountId = entity.Id,
                    x.Id,
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.SessionToken,
                    x.DeviceInfo
                }), transaction);

            await _db.ExecuteAsync(
                $"""
                DELETE FROM Sessions 
                WHERE 
                    AccountId = @AccountId AND 
                    Id <> ALL(@SessionIds)
                """,
                new
                {
                    AccountId = entity.Id,
                    SessionIds = entity.Sessions.Select(x => x.Id).ToList()
                }, transaction);

            transaction.Commit();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var transaction = _db.BeginTransaction();

            await _db.ExecuteAsync(
                """
                DELETE FROM Sessions WHERE AccountId = @Id
                """,
                new { Id = id }, transaction);

            await _db.ExecuteAsync(
                """
                DELETE FROM Credentials WHERE AccountId = @Id
                """,
                new { Id = id }, transaction);

            await _db.ExecuteAsync(
                """
                DELETE FROM Accounts WHERE Id = @Id
                """,
                new { Id = id }, transaction);

            transaction.Commit();
        }
    }
}
