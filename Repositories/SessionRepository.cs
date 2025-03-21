using Dapper;
using System.Data;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Repositories
{
    public class SessionRepository : ISessionRepository, IDisposable
    {
        private readonly IDbConnection _cn;

        public SessionRepository(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<Session?> GetAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"SELECT sessions.* FROM sessions WHERE Id=@Id";
            return await _cn.QuerySingleOrDefaultAsync<Session>(sql, parameters);
        }

        public async Task<ulong?> CreateAsync(Session entity)
        {
            var parameters = new
            {
                AccountId = entity.AccountId,
                ExpiresAt = entity.ExpiresAt,
                SessionToken = entity.SessionToken
            };

            string sql = @"INSERT INTO sessions (AccountId,SessionToken,ExpiresAt) 
                           VALUES (@AccountId,@SessionToken,@ExpiresAt);
                           SELECT Id FROM sessions WHERE Id = LAST_INSERT_ID();";

            return await _cn.QuerySingleOrDefaultAsync<ulong>(sql, parameters);
        }

        public async Task<ulong?> UpdateAsync(Session entity)
        {
            if (!entity.Id.HasValue)
            {
                throw new ArgumentNullException(nameof(Session.Id));
            }

            string sql = @"UPDATE sessions
                           SET AccountId = @AccountId,
                               SessionToken = @SessionToken,
                               ExpiresAt = @ExpiresAt
                           WHERE Id = @Id";

            return await _cn.ExecuteAsync(sql, entity) == 1 ? entity.Id : null;
        }

        public async Task<bool> DeleteAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"DELETE FROM sessions WHERE Id = @Id";
            return await _cn.ExecuteAsync(sql, parameters) == 1;
        }

        public async Task<Session?> GetSessionByToken(string token)
        {
            var parameters = new
            {
                SessionToken = token
            };

            string sql = @"SELECT sessions.* FROM sessions WHERE SessionToken=@SessionToken";
            return await _cn.QuerySingleOrDefaultAsync<Session>(sql, parameters);
        }

        public void Dispose()
        {
            _cn.Dispose();
        }
    }
}
