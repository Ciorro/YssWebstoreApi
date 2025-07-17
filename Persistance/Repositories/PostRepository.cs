using Dapper;
using System.Data;
using YssWebstoreApi.Models;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Persistance.Repositories
{
    public class PostRepository : IRepository<Post>
    {
        private readonly IDbConnection _db;

        public PostRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Post?> GetAsync(Guid id)
        {
            Post? result = null;

            await _db.QueryAsync<Post, Guid, Post>(
                """
                SELECT
                    Posts.Id,
                    Posts.CreatedAt,
                    Posts.UpdatedAt,
                    Posts.AccountId,
                    Posts.ProjectId,
                    Posts.Title,
                    Posts.Content,
                    Images.Id
                FROM
                    Posts
                    INNER JOIN Accounts ON Accounts.Id = Posts.AccountId
                    LEFT JOIN Images ON Images.Id = Posts.ImageId
                WHERE
                    Posts.Id = @Id
                """,
                (post, image) =>
                {
                    result ??= post;
                    result.ImageResourceId = image;
                    return result;
                },
                new
                {
                    Id = id
                });

            return result;
        }

        public async Task InsertAsync(Post entity)
        {
            await _db.ExecuteAsync(
                $"""
                INSERT INTO Posts (
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    AccountId,
                    Title,
                    Content,
                    ImageId,
                    ProjectId
                ) VALUES (
                    @{nameof(Post.Id)},
                    @{nameof(Post.CreatedAt)},
                    @{nameof(Post.UpdatedAt)},
                    @{nameof(Post.AccountId)},
                    @{nameof(Post.Title)},
                    @{nameof(Post.Content)},
                    @{nameof(Post.ImageResourceId)},
                    @{nameof(Post.TargetProjectId)}
                )
                """, entity);
        }

        public async Task UpdateAsync(Post entity)
        {
            await _db.ExecuteAsync(
                $"""
                UPDATE Posts
                SET Id = @{nameof(Post.Id)},
                    CreatedAt = @{nameof(Post.CreatedAt)},
                    UpdatedAt = @{nameof(Post.UpdatedAt)},
                    AccountId = @{nameof(Post.AccountId)},
                    Title = @{nameof(Post.Title)},
                    Content = @{nameof(Post.Content)},
                    ImageId = @{nameof(Post.ImageResourceId)},
                    ProjectId = @{nameof(Post.TargetProjectId)}
                WHERE
                    Id = @{nameof(Post.Id)}
                """, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.ExecuteAsync(
                """
                DELETE FROM Resources WHERE Id = @Id
                """,
                new { Id = id });
        }
    }
}
