using Dapper;
using YssWebstoreApi.Database;
using YssWebstoreApi.Models;
using YssWebstoreApi.Models.Api;
using YssWebstoreApi.Models.Query;
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

        public async Task<IEnumerable<Product>> Search(SearchParams searchParams, SortParams sortParams, Pagination pagination)
        {
            using (var cn = _dbConnectionFactory.Create())
            {
                var builder = new SqlBuilder();
                var template = builder.AddTemplate(
                    @"SELECT products.* FROM products 
                    LEFT JOIN tagbindings ON products.Id = tagbindings.ItemId
                    LEFT JOIN tags ON tagbindings.TagId = tags.Id
                    /**where**/
                    GROUP BY Id
                    /**orderby**/
                    LIMIT @limit OFFSET @offset");

                if (!string.IsNullOrEmpty(searchParams.SearchQuery))
                {
                    builder.Where("products.Name LIKE @searchQuery");
                }
                if (searchParams.Tags?.Length > 0)
                {
                    builder.Where("tags.Name IN @tags");
                }
                if (searchParams.AccountId.HasValue)
                {
                    builder.Where("products.AccountId = @accountId");
                }

                if (!string.IsNullOrEmpty(sortParams.OrderBy))
                {
                    var orderProperty = sortParams.OrderBy.ToLower() switch
                    {
                        "createdat" => "CreatedAt",
                        "updatedat" => "UpdatedAt",
                        _ => throw new Exception("Invalid parameters.")
                    };

                    builder.OrderBy($"{orderProperty} {(sortParams.SortBy == "ASC" ? "ASC" : "DESC")}");
                }

                return await cn.QueryAsync<Product>(template.RawSql, new
                {
                    searchQuery = $"%{searchParams.SearchQuery}%",
                    searchParams.Tags,
                    searchParams.AccountId,
                    offset = pagination.PageSize * pagination.Page,
                    limit = pagination.PageSize
                });
            }
        }
    }
}
