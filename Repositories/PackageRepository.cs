using Dapper;
using YssWebstoreApi.Database;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly TimeProvider _timeProvider;

        public PackageRepository(IDbConnectionFactory dbConnectionFactory, TimeProvider timeProvider)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _timeProvider = timeProvider;
        }

        public async Task<Package?> GetAsync(uint id)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "select * from Packages where Id=@id",
                    parameters: new { id }
                );

                return await cn.QuerySingleOrDefaultAsync<Package>(command);
            }
        }

        public async Task<IEnumerable<Package>> GetAllAsync()
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "select * from Packages"
                );

                return await cn.QueryAsync<Package>(command);
            }
        }

        public async Task<bool> CreateAsync(Package entity)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: @"insert into Packages (ProductId, Name, Version, DownloadUrl, TargetOS) 
                                   values (@ProductId, @Name, @Version, @DownloadUrl, @TargetOS)",
                    parameters: entity
                );

                return await cn.ExecuteAsync(command) == 1;
            }
        }

        public async Task<bool> UpdateAsync(uint id, Package entity)
        {
            entity.UpdatedAt = _timeProvider.GetUtcNow();

            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "update Packages set UpdatedAt=@UpdatedAt, Name=@Name where Id=@id",
                    parameters: new
                    {
                        entity.UpdatedAt,
                        entity.Name,
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
                    commandText: "delete from Packages where Id=@id",
                    parameters: new { id }
                );

                return await cn.ExecuteAsync(command) == 1;
            }
        }

        public async Task<IEnumerable<Package>> GetPackagesByProductAsync(uint productId)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "select * from Packages where ProductId=@ProductId",
                    parameters: new { productId }
                );

                return await cn.QueryAsync<Package>(command);
            }
        }
    }
}
