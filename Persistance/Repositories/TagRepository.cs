using Dapper;
using System.Data;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Entities.Tags;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Persistance.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly IDbConnection _db;
        private readonly TimeProvider _timeProvider;

        public TagRepository(IDbConnection dbConnection, TimeProvider timeProvider)
        {
            _db = dbConnection;
            _timeProvider = timeProvider;
        }

        public async Task<TagEntity?> GetAsync(Guid id)
        {
            return await _db.QuerySingleOrDefaultAsync<TagEntity>(
                """
                SELECT
                    Tag.Id,
                    Tag.CreatedAt,
                    Tag.UpdatedAt,
                    Tag.Tag
                FROM Tags WHERE Tag.Id = @Id
                """,
                new
                {
                    Id = id
                });
        }

        public async Task InsertAsync(TagEntity entity)
        {
            await _db.ExecuteAsync(
                $"""
                INSERT INTO Tags (
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    Tag
                ) VALUES (
                    @{nameof(TagEntity.Id)},
                    @{nameof(TagEntity.CreatedAt)},
                    @{nameof(TagEntity.UpdatedAt)},
                    @{nameof(TagEntity.Tag)}                
                )
                """, entity);
        }

        public async Task UpdateAsync(TagEntity entity)
        {
            await _db.ExecuteAsync(
                $"""
                UPDATE Tags
                SET Id = @{nameof(TagEntity.Id)},
                    CreatedAt = @{nameof(TagEntity.CreatedAt)},
                    UpdatedAt = @{nameof(TagEntity.UpdatedAt)},
                    Tag = @{nameof(TagEntity.Tag)}
                WHERE Tag.Id = @{nameof(TagEntity.Id)}
                """, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.ExecuteAsync(
                """
                DELETE FROM Tags WHERE Tag.Id = @Id
                """,
                new
                {
                    Id = id
                });
        }

        public async Task<IList<TagEntity>> GetAndInsert(IEnumerable<Tag> tags)
        {
            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;

            await _db.ExecuteAsync(
                $"""
                INSERT INTO Tags (
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    Tag
                ) VALUES (
                    @{nameof(TagEntity.Id)},
                    @{nameof(TagEntity.CreatedAt)},
                    @{nameof(TagEntity.UpdatedAt)},
                    @{nameof(TagEntity.Tag)}                
                ) ON CONFLICT (Tag) DO NOTHING
                """,
                tags.Select(
                    x => new
                    {
                        Id = Guid.CreateVersion7(creationTime),
                        CreatedAt = creationTime,
                        UpdatedAt = creationTime,
                        Tag = x
                    }));

            var result = await _db.QueryAsync<TagEntity>(
                """
                SELECT
                    Tags.Id,
                    Tags.CreatedAt,
                    Tags.UpdatedAt,
                    Tags.Tag
                FROM 
                    Tags
                WHERE Tags.Tag = ANY(@Tags)
                """,
                new
                {
                    Tags = tags
                        .Select(x => x.ToString())
                        .ToList()
                });

            return result.ToList();
        }
    }
}
