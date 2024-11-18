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

            string sql = @"SELECT reviews.* FROM packages WHERE Id=@Id";
            return await _cn.QuerySingleOrDefaultAsync(sql, parameters);
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

        public async Task<ulong?> UpdateAsync(ulong id, Review entity)
        {
            var parameters = new
            {
                Id = id,
                Rate = entity.Rate,
                Content = entity.Content,
            };

            string sql = @"UPDATE reviews
                           SET Rate = @Rate,
                               Content = @Content
                           WHERE Id = @Id";

            return await _cn.ExecuteAsync(sql, parameters) == 1 ? id : null;
        }

        public async Task<ulong?> DeleteAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"DELETE FROM reviews WHERE Id = @Id RETURNING Id";
            return await _cn.QuerySingleOrDefaultAsync<ulong>(sql, parameters);
        }

        public void Dispose()
        {
            _cn.Dispose();
        }
    }
}
