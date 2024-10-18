using Dapper;
using System.Data;
using YssWebstoreApi.Database;
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
            return await _cn.QuerySingleOrDefaultAsync(sql, parameters);
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

        public async Task<ulong?> UpdateAsync(ulong id, Account entity)
        {
            var parameters = new
            {
                Id = id,
                UniqueName = entity.UniqueName,
                DisplayName = entity.DisplayName,
                Status = entity.Status
            };

            string sql = @"UPDATE accounts
                           SET UniqueName = @UniqueName,
                               DisplayName = @DisplayName,
                               Status = @Status
                           WHERE Id = @Id";

            return await _cn.ExecuteAsync(sql, parameters) == 1 ? id : null;
        }

        public async Task<ulong?> DeleteAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"DELETE FROM accounts WHERE Id = @Id RETURNING Id";
            return await _cn.QuerySingleOrDefaultAsync<ulong>(sql, parameters);
        }

        public void Dispose()
        {
            _cn.Dispose();
        }
    }
}
