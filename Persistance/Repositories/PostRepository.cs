using Dapper;
using System.Data;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Persistance.Repositories
{
    public class PostRepository : IRepository<Post>
    {
        private readonly IDbConnection _db;

        public PostRepository(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<Post?> GetAsync(Guid id)
        {
            var posts = await _db.QueryAsync<Post, Resource, Post>(
                """
                SELECT
                    Posts.Id,
                    Posts.CreatedAt,
                    Posts.UpdatedAt,
                    Posts.AccountId,
                    Posts.Title,
                    Posts.Content,
                    Posts.TargetProjectId,
                    Resources.Id,
                    Resources.CreatedAt,
                    Resources.UpdatedAt,
                    Resources.Path,
                    Resources.PublicUrl
                FROM
                    Posts
                    INNER JOIN Accounts ON Accounts.Id = Posts.AccountId
                    LEFT JOIN Resources ON Resources.Id = Posts.ImageResourceId
                WHERE
                    Posts.Id = @Id
                """,
                (post, image) =>
                {
                    post.Image = image;
                    return post;
                },
                new
                {
                    Id = id
                });

            return posts.SingleOrDefault();
        }

        public async Task InsertAsync(Post entity)
        {
            using var transaction = _db.BeginTransaction();

            await _db.ExecuteAsync(
                $"""
                INSERT INTO Posts (
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    AccountId,
                    Title,
                    Content,
                    ImageResourceId,
                    TargetProjectId
                ) VALUES (
                    @{nameof(Post.Id)},
                    @{nameof(Post.CreatedAt)},
                    @{nameof(Post.UpdatedAt)},
                    @{nameof(Post.AccountId)},
                    @{nameof(Post.Title)},
                    @{nameof(Post.Content)},
                    @ImageResourceId,
                    @{nameof(Post.TargetProjectId)}
                )
                """,
                new
                {
                    entity.Id,
                    entity.CreatedAt,
                    entity.UpdatedAt,
                    entity.AccountId,
                    entity.Title,
                    entity.Content,
                    entity.TargetProjectId,
                    ImageResourceId = entity.Image?.Id,
                }, transaction);

            if (entity.Image is not null)
            {
                await _db.ExecuteAsync(
                    $"""
                    INSERT INTO Resources (
                        Id,
                        CreatedAt,
                        UpdatedAt,
                        Path,
                        PublicUrl
                    ) VALUES (
                        @{nameof(Resource.Id)}, 
                        @{nameof(Resource.CreatedAt)},   
                        @{nameof(Resource.UpdatedAt)}, 
                        @{nameof(Resource.Path)},
                        @{nameof(Resource.PublicUrl)}
                    )
                    """, 
                    entity.Image, transaction);
            }

            transaction.Commit();
        }

        public async Task UpdateAsync(Post entity)
        {
            using var transaction = _db.BeginTransaction();

            if (entity.Image is null)
            {
                await _db.ExecuteAsync(
                    """
                        DELETE FROM Resources USING Posts
                        WHERE Posts.Id = @Id
                          AND Resources.Id = Posts.ImageResourceId
                    """,
                    new { entity.Id }, transaction);
            }
            else
            {
                await _db.ExecuteAsync(
                    $"""
                    INSERT INTO Resources (
                        Id,
                        CreatedAt,
                        UpdatedAt,
                        Path,
                        PublicUrl
                    ) VALUES (
                        @{nameof(Resource.Id)}, 
                        @{nameof(Resource.CreatedAt)},   
                        @{nameof(Resource.UpdatedAt)}, 
                        @{nameof(Resource.Path)},
                        @{nameof(Resource.PublicUrl)}
                    ) ON CONFLICT (Id) DO UPDATE
                    SET Id = @{nameof(Resource.Id)},
                        CreatedAt = @{nameof(Resource.CreatedAt)},
                        UpdatedAt = @{nameof(Resource.UpdatedAt)},
                        Path = @{nameof(Resource.Path)},
                        PublicUrl = @{nameof(Resource.PublicUrl)}
                    """, 
                    entity.Image, transaction);
            }

            await _db.ExecuteAsync(
                $"""
                UPDATE Posts
                SET Id = @{nameof(Post.Id)},
                    CreatedAt = @{nameof(Post.CreatedAt)},
                    UpdatedAt = @{nameof(Post.UpdatedAt)},
                    AccountId = @{nameof(Post.AccountId)},
                    Title = @{nameof(Post.Title)},
                    Content = @{nameof(Post.Content)},
                    ImageResourceId = @ImageResourceId,
                    TargetProjectId = @{nameof(Post.TargetProjectId)}
                WHERE
                    Id = @{nameof(Post.Id)}
                """, 
                new
                {
                    entity.Id,
                    entity.CreatedAt,
                    entity.UpdatedAt,
                    entity.AccountId,
                    entity.Title,
                    entity.Content,
                    entity.TargetProjectId,
                    ImageResourceId = entity.Image?.Id
                }, transaction);

            transaction.Commit();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.ExecuteAsync(
                """
                    DELETE FROM Resources USING Posts
                    WHERE Posts.Id = @Id
                        AND Resources.Id = Posts.ImageResourceId
                """,
                new { Id = id });

            await _db.ExecuteAsync(
                """
                DELETE FROM Posts WHERE Id = @Id
                """,
                new { Id = id });
        }
    }
}
