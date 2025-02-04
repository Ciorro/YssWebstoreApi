using Dapper;
using System.Data;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Repositories
{
    public class ProductRepository : IRepository<Product>, IDisposable
    {
        private readonly IDbConnection _cn;

        public ProductRepository(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<Product?> GetAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"SELECT products.* FROM products WHERE Id=@Id";
            return await _cn.QuerySingleOrDefaultAsync<Product>(sql, parameters);
        }

        public async Task<ulong?> CreateAsync(Product entity)
        {
            var parameters = new
            {
                AccountId = entity.AccountId,
                Name = entity.Name,
                Description = entity.Description,
                SourceUrl = entity.SourceUrl,
                Tags = string.Join(' ', entity.Tags)
            };

            string sql = @"INSERT INTO products (AccountId,Name,Description,SourceUrl,Tags) 
                           VALUES (@AccountId,@Name,@Description,@SourceUrl,@Tags) RETURNING Id";

            return await _cn.QuerySingleOrDefaultAsync<ulong>(sql, parameters);
        }

        public async Task<ulong?> UpdateAsync(Product entity)
        {
            if (!entity.Id.HasValue)
            {
                throw new ArgumentNullException(nameof(Product.Id));
            }

            string sql = @"UPDATE products
                           SET AccountId = @AccountId,
                               Name = @Name,
                               Description = @Description,
                               SourceUrl = @SourceUrl,
                               Tags = @Tags,
                               IsPinned = @IsPinned
                           WHERE Id = @Id";

            return await _cn.ExecuteAsync(sql, entity) == 1 ? entity.Id : null;
        }

        public async Task<bool> DeleteAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"DELETE FROM products WHERE Id = @Id";
            return await _cn.ExecuteAsync(sql, parameters) == 1;
        }

        public void Dispose()
        {
            _cn.Dispose();
        }
    }
}
