using Dapper;
using System.Data;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Persistance.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnection _db;

        public AccountRepository(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<Account?> GetAsync(Guid id)
        {
            using var results = await _db.QueryMultipleAsync(
                """
                SELECT
                    Accounts.Id,
                	Accounts.CreatedAt,
                	Accounts.UpdatedAt,
                	Accounts.UniqueName,
                	Accounts.DisplayName,
                	Accounts.StatusText
                FROM Accounts WHERE Accounts.Id = @Id;

                SELECT 
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
                	Credentials.IsVerified
                FROM Credentials WHERE Credentials.AccountId = @Id;

                SELECT 
                    Sessions.Id,
                	Sessions.CreatedAt,
                	Sessions.UpdatedAt,
                	Sessions.SessionToken,
                	Sessions.DeviceInfo
                FROM Sessions WHERE Sessions.AccountId = @Id;

                SELECT
                    Resources.Id,
                    Resources.CreatedAt,
                    Resources.UpdatedAt,
                    Resources.Path,
                    Resources.PublicUrl
                FROM 
                    Accounts JOIN Resources ON Resources.Id = Accounts.AvatarResourceId
                WHERE
                    Accounts.Id = @Id;
                """, new
                {
                    Id = id
                });

            var account = await results.ReadSingleOrDefaultAsync<Account>();
            if (account is null)
            {
                return null;
            }

            account.Credentials = await results.ReadSingleAsync<Credentials>();
            account.Sessions = [.. await results.ReadAsync<Session>()];
            account.Avatar = await results.ReadSingleOrDefaultAsync<Resource>();

            return account;
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
                """, entity, transaction);

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

            await InsertSessions(entity, transaction);
            await InsertAvatar(entity, transaction);

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

            await UpsertSessions(entity, transaction);
            await UpsertAvatar(entity, transaction);

            transaction.Commit();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var transaction = _db.BeginTransaction();

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

            await DeleteSessions(id, transaction);
            await DeleteAvatar(id, transaction);

            transaction.Commit();
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            Guid? accountId = await _db.QuerySingleOrDefaultAsync<Guid>(
                """
                SELECT Credentials.AccountId FROM Credentials
                WHERE
                    Credentials.Email = @Email
                """,
                new
                {
                    Email = email
                });

            return accountId is not null ?
                await GetAsync(accountId.Value) : null;
        }

        public async Task<Account?> GetByUniqueNameAsync(string uniqueName)
        {
            Guid? accountId = await _db.QuerySingleOrDefaultAsync<Guid>(
                """
                SELECT Accounts.Id FROM Accounts
                WHERE
                    Accounts.UniqueName = @UniqueName
                """,
                new
                {
                    UniqueName = uniqueName
                });

            return accountId is not null ?
                await GetAsync(accountId.Value) : null;
        }

        private async Task DeleteSessions(Guid entityId, IDbTransaction transaction)
        {
            await _db.ExecuteAsync(
                """
                DELETE FROM Sessions WHERE AccountId = @Id
                """,
                new { Id = entityId }, transaction);
        }

        private async Task InsertSessions(Account entity, IDbTransaction transaction)
        {
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
        }

        private async Task UpsertSessions(Account entity, IDbTransaction transaction)
        {
            await DeleteSessions(entity.Id, transaction);
            await InsertSessions(entity, transaction);
        }

        private async Task DeleteAvatar(Guid entityId, IDbTransaction transaction)
        {
            await _db.ExecuteAsync(
                """
                DELETE FROM Resources USING Accounts
                WHERE Accounts.Id = @AccountId
                  AND Accounts.AvatarResourceId = Resources.Id;

                UPDATE Accounts 
                SET 
                    AvatarResourceId = NULL
                WHERE
                    Id = @AccountId;
                
                """, new { AccountId = entityId }, transaction);
        }

        private async Task InsertAvatar(Account entity, IDbTransaction transaction)
        {
            if (entity.Avatar is null)
            {
                return;
            }

            await _db.ExecuteAsync(
                    $"""
                    INSERT INTO Resources (
                        Id,
                        CreatedAt,
                        UpdatedAt,
                        Path,
                        PublicUrl
                    ) VALUES (
                        @{nameof(Resource.Id)}, 
                        @{nameof(Resource.CreatedAt)},   
                        @{nameof(Resource.UpdatedAt)}, 
                        @{nameof(Resource.Path)},
                        @{nameof(Resource.PublicUrl)}
                    );

                    UPDATE Accounts
                    SET
                        AvatarResourceId = @{nameof(Resource.Id)}
                    WHERE
                        Id = @AccountId;
                    """,
                    new
                    {
                        entity.Avatar.Id,
                        entity.Avatar.CreatedAt,
                        entity.Avatar.UpdatedAt,
                        entity.Avatar.Path,
                        entity.Avatar.PublicUrl,
                        AccountId = entity.Id
                    },
                    transaction);
        }

        private async Task UpsertAvatar(Account entity, IDbTransaction transaction)
        {
            await DeleteAvatar(entity.Id, transaction);
            await InsertAvatar(entity, transaction);
        }
    }
}
