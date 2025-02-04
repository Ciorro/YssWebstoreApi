using Dapper;
using System.Data;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Repositories
{
    public class ReviewRepository : IRepository<Review>, IDisposable
    {
        private readonly IDbConnection _cn;

        public ReviewRepository(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<Review?> GetAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"SELECT reviews.* FROM reviews WHERE Id=@Id";
            return await _cn.QuerySingleOrDefaultAsync<Review>(sql, parameters);
        }

        public async Task<ulong?> CreateAsync(Review entity)
        {
            var parameters = new
            {
                AccountId = entity.AccountId,
                ProductId = entity.ProductId,
                Rate = entity.Rate,
                Content = entity.Content,
            };

            string sql = @"INSERT INTO reviews (AccountId,ProductId,Rate,Content) 
                           VALUES (@AccountId,@ProductId,@Rate,@Content) RETURNING Id";

            return await _cn.QuerySingleOrDefaultAsync<ulong>(sql, parameters);
        }

        public async Task<ulong?> UpdateAsync(Review entity)
        {
            if (!entity.Id.HasValue)
            {
                throw new ArgumentNullException(nameof(Review.Id));
            }

            string sql = @"UPDATE reviews
                           SET AccountId = @AccountId,
                               ProductId = @ProductId,
                               Rate = @Rate,
                               Content = @Content
                           WHERE Id = @Id";

            return await _cn.ExecuteAsync(sql, entity) == 1 ? entity.Id : null;
        }

        public async Task<bool> DeleteAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"DELETE FROM reviews WHERE Id = @Id";
            return await _cn.ExecuteAsync(sql, parameters) == 1;
        }

        public void Dispose()
        {
            _cn.Dispose();
        }
    }
}
