using Dapper;
using YssWebstoreApi.Database;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly TimeProvider _timeProvider;

        public ProductRepository(IDbConnectionFactory dbConnectionFactory, TimeProvider timeProvider)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _timeProvider = timeProvider;
        }

        public async Task<Product?> GetAsync(uint id)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "select * from Products where Id=@id",
                    parameters: new { id }
                );

                return await cn.QuerySingleOrDefaultAsync<Product>(command);
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "select * from Products"
                );

                return await cn.QueryAsync<Product>(command);
            }
        }

        public async Task<bool> CreateAsync(Product entity)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: @"insert into Products (AccountId, Name, Description, SourceUrl) 
                                   values (@AccountId, @Name, @Description, @SourceUrl)",
                    parameters: entity
                );

                return await cn.ExecuteAsync(command) == 1;
            }
        }

        public async Task<bool> UpdateAsync(uint id, Product entity)
        {
            entity.UpdatedAt = _timeProvider.GetUtcNow();

            using (var cn = _dbConnectionFactory.Create())
            {
                var command = new CommandDefinition(
                    commandText: "update Products set UpdatedAt=@UpdatedAt, Name=@Name, Description=@Description, SourceUrl=@SourceUrl where Id=@id",
                    parameters: new
                    {
                        entity.UpdatedAt,
                        entity.Name,
                        entity.Description,
                        entity.SourceUrl,
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
                    commandText: "delete from Products where Id=@id",
                    parameters: new { id }
                );

                return await cn.ExecuteAsync(command) == 1;
            }
        }
    }
}
