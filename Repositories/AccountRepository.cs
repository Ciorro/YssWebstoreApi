using Dapper;
using YssWebstoreApi.Database;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly TimeProvider _timeProvider;

        public AccountRepository(IDbConnectionFactory dbConnectionFactory, TimeProvider timeProvider)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _timeProvider = timeProvider;
        }

        public async Task<Account?> GetAsync(uint id)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "select * from Accounts where Id=@id",
                    parameters: new { id }
                );

                return await cn.QuerySingleOrDefaultAsync<Account>(command);
            }
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "select * from Accounts"
                );

                return await cn.QueryAsync<Account>(command);
            }
        }

        public async Task<bool> CreateAsync(Account entity)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: @"insert into Accounts (UniqueName, DisplayName) 
                                   values (@UniqueName, @DisplayName)",
                    parameters: entity
                );

                return await cn.ExecuteAsync(command) == 1;
            }
        }

        public async Task<bool> UpdateAsync(uint id, Account entity)
        {
            entity.UpdatedAt = _timeProvider.GetUtcNow();

            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "update Accounts set UpdatedAt=@UpdatedAt, UniqueName=@uniqueName, DisplayName=@displayName where Id=@id",
                    parameters: new
                    {
                        entity.UpdatedAt,
                        entity.UniqueName,
                        entity.DisplayName,
                        id
                    }
                );

                return await cn.ExecuteAsync(command) == 1;
            }
        }

        public async Task<bool> DeleteAsync(uint id)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "delete from Accounts where Id=@id",
                    parameters: new { id }
                );

                return await cn.ExecuteAsync(command) == 1;
            }
        }

        public async Task<Account?> GetByUniqueNameAsync(string uniqueName)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "select * from Accounts where UniqueName=@uniqueName",
                    parameters: new { uniqueName }
                );

                return await cn.QuerySingleOrDefaultAsync<Account>(command);
            }
        }
    }
}
