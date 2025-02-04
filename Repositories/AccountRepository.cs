using Dapper;
using System.Data;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Repositories
{
    public class AccountRepository : IRepository<Account>, IDisposable
    {
        private readonly IDbConnection _cn;

        public AccountRepository(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<Account?> GetAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = "SELECT accounts.* FROM accounts WHERE Id=@Id";
            return await _cn.QuerySingleOrDefaultAsync<Account>(sql, parameters);
        }

        public async Task<ulong?> CreateAsync(Account entity)
        {
            var parameters = new
            {
                UniqueName = entity.UniqueName,
                DisplayName = entity.DisplayName
            };

            string sql = @"INSERT INTO accounts (UniqueName,DisplayName) 
                           VALUES (@UniqueName,@DisplayName) RETURNING Id";

            return await _cn.QuerySingleOrDefaultAsync<ulong>(sql, parameters);
        }

        public async Task<ulong?> UpdateAsync(Account entity)
        {
            if (!entity.Id.HasValue)
            {
                throw new ArgumentNullException(nameof(Account.Id));
            }

            string sql = @"UPDATE accounts
                           SET UniqueName = @UniqueName,
                               DisplayName = @DisplayName,
                               Status = @Status
                           WHERE Id = @Id";

            return await _cn.ExecuteAsync(sql, entity) == 1 ? entity.Id : null;
        }

        public async Task<bool> DeleteAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"DELETE FROM accounts WHERE Id = @Id";
            return await _cn.ExecuteAsync(sql, parameters) == 1;
        }

        public void Dispose()
        {
            _cn.Dispose();
        }
    }
}
