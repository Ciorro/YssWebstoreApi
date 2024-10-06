using Dapper;
using YssWebstoreApi.Database;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Repositories
{
    public class CredentialRepository : ICredentialRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public CredentialRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Credentials?> GetAsync(uint id)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "select * from credentials where Id=@id",
                    parameters: new { id }
                );

                return await cn.QuerySingleOrDefaultAsync<Credentials>( command );
            }
        }

        public async Task<IEnumerable<Credentials>> GetAllAsync()
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "select * from credentials"
                );

                return await cn.QueryAsync<Credentials>(command);
            }
        }

        public async Task<bool> CreateAsync(Credentials entity)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: @"insert into credentials (AccountId, Email, PasswordHash, PasswordSalt) 
                                    values (@AccountId, @Email, @PasswordHash, @PasswordSalt)",
                    parameters: entity
                );

                return await cn.ExecuteAsync(command) == 1;
            }
        }

        public async Task<bool> UpdateAsync(uint id, Credentials entity)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: @"update credentials set 
                                   Email = @Email, 
                                   PasswordHash = @PasswordHash, 
                                   PasswordSalt = @PasswordSalt, 
                                   RefreshToken = @RefreshToken, 
                                   VerificationCode = @VerificationCode, 
                                   RefreshTokenExpiresAt = @RefreshTokenExpiresAt,
                                   VerificationCodeExpiresAt = @VerificationCodeExpiresAt
                                   where Id=@Id",
                    parameters: entity
                );

                return await cn.ExecuteAsync(command) == 1;
            }
        }

        public Task<bool> DeleteAsync(uint id)
        {
            throw new NotImplementedException();
        }

        public async Task<Credentials?> GetByEmailAsync(string email)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "select * from credentials where Email=@email",
                    parameters: new { email }
                );

                return await cn.QuerySingleOrDefaultAsync<Credentials>(command);
            }
        }

        public async Task<Credentials?> GetByAccountAsync(uint accountId)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "select * from credentials where AccountId=@accountId",
                    parameters: new { accountId }
                );

                return await cn.QuerySingleOrDefaultAsync<Credentials>(command);
            }
        }
    }
}
