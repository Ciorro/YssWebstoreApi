using Dapper;
using System.Data;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Repositories
{
    public class CredentialsRepository : ICredentialsRepository, IDisposable
    {
        private readonly IDbConnection _cn;

        public CredentialsRepository(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<Credentials?> GetAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = "SELECT credentials.* FROM credentials WHERE Id=@Id";
            return await _cn.QuerySingleOrDefaultAsync<Credentials>(sql, parameters);
        }

        public async Task<ulong?> CreateAsync(Credentials entity)
        {
            var parameters = new
            {
                AccountId = entity.AccountId,
                Email = entity.Email,
                PasswordHash = entity.PasswordHash,
                PasswordSalt = entity.PasswordSalt
            };

            string sql = @"INSERT INTO credentials (AccountId,Email,PasswordHash,PasswordSalt) 
                           VALUES (@AccountId,@Email,@PasswordHash,@PasswordSalt);
                           SELECT Id FROM credentials WHERE Id = LAST_INSERT_ID();";

            return await _cn.QuerySingleOrDefaultAsync<ulong>(sql, parameters);
        }

        public async Task<ulong?> UpdateAsync(Credentials entity)
        {
            if (!entity.Id.HasValue)
            {
                throw new ArgumentNullException(nameof(Credentials.Id));
            }

            string sql = @"UPDATE credentials
                           SET Email = @Email,
                               PasswordHash = @PasswordHash,
                               PasswordSalt = @PasswordSalt,
                               VerificationCode = @VerificationCode,
                               VerificationCodeExpiresAt = @VerificationCodeExpiresAt,
                               PasswordResetCode = @PasswordResetCode,
                               PasswordResetCodeExpiresAt = @PasswordResetCodeExpiresAt
                           WHERE Id = @Id";

            return await _cn.ExecuteAsync(sql, entity) == 1 ? entity.Id : null;
        }

        public async Task<bool> DeleteAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"DELETE FROM credentials WHERE Id = @Id";
            return await _cn.ExecuteAsync(sql, parameters) == 1;
        }

        public async Task<Credentials?> GetByAccountIdAsync(ulong id)
        {
            var parameters = new
            {
                AccountId = id
            };

            string sql = "SELECT credentials.* FROM credentials WHERE AccountId=@AccountId";
            return await _cn.QuerySingleOrDefaultAsync<Credentials>(sql, parameters);
        }

        public async Task<Credentials?> GetByEmailAsync(string email)
        {
            var parameters = new
            {
                Email = email
            };

            string sql = "SELECT credentials.* FROM credentials WHERE Email=@Email";
            return await _cn.QuerySingleOrDefaultAsync<Credentials>(sql, parameters);
        }

        public void Dispose()
        {
            _cn.Dispose();
        }
    }
}
