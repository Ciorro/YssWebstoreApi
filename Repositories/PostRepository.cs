using Dapper;
using System.Data;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Repositories
{
    public class PostRepository : IRepository<Post>
    {
        private readonly IDbConnection _cn;

        public PostRepository(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<Post?> GetAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"SELECT posts.* FROM posts WHERE Id=@Id";
            return await _cn.QuerySingleOrDefaultAsync<Post>(sql, parameters);
        }

        public async Task<ulong?> CreateAsync(Post entity)
        {
            var parameters = new
            {
                Title = entity.Title,
                Content = entity.Content,
                ImageId = entity.ImageId,
                AccountId = entity.AccountId,
                ProductId = entity.ProductId
            };

            string sql = @"INSERT INTO posts (Title,Content,ImageId,AccountId,ProductId) 
                           VALUES (@Title,@Content,@ImageId,@AccountId,@ProductId);
                           SELECT Id FROM posts WHERE Id = LAST_INSERT_ID();";

            return await _cn.QuerySingleOrDefaultAsync<ulong>(sql, parameters);
        }

        public async Task<ulong?> UpdateAsync(Post entity)
        {
            if (!entity.Id.HasValue)
            {
                throw new ArgumentNullException(nameof(Post.Id));
            }

            string sql = @"UPDATE posts
                           SET Title = @Title,
                               Content = @Content,
                               ImageId = @ImageId,
                               AccountId = @AccountId,
                               ProductId = @ProductId
                           WHERE Id = @Id";

            return await _cn.ExecuteAsync(sql, entity) == 1 ? entity.Id : null;
        }

        public async Task<bool> DeleteAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"DELETE FROM posts WHERE Id = @Id";
            return await _cn.ExecuteAsync(sql, parameters) == 1;
        }
    }
}
