using Dapper;
using System.Data;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Persistance.Repositories
{
    public class ResourceRepository : IRepository<Resource>
    {
        private readonly IDbConnection _db;

        public ResourceRepository(IDbConnection dbConnection)
        {
            _db = dbConnection;
            _db.Open();
        }

        public async Task<Resource?> GetAsync(Guid id)
        {
            return await _db.QuerySingleOrDefaultAsync<Resource>(
                """
                SELECT 
                    Resources.Id,
                    Resources.CreatedAt,
                    Resources.UpdatedAt,
                    Resources.AccountId,
                    Resources.Size,
                    Resources.Path
                FROM Resources
                WHERE Resources.Id = @Id
                """,
                new
                {
                    Id = id
                });
        }

        public async Task InsertAsync(Resource entity)
        {
            await _db.ExecuteAsync(
                $"""
                INSERT INTO Resources (
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    AccountId,
                    Path,
                    Size
                ) VALUES (
                    @{nameof(Resource.Id)},
                    @{nameof(Resource.CreatedAt)},
                    @{nameof(Resource.UpdatedAt)},
                    @{nameof(Resource.AccountId)},
                    @{nameof(Resource.Path)},
                    @{nameof(Resource.Size)}
                );
                """, entity);
        }

        public async Task UpdateAsync(Resource entity)
        {
            await _db.ExecuteAsync(
                $"""
                UPDATE Resources
                SET Id = @{nameof(Resource.Id)},
                    CreatedAt = @{nameof(Resource.CreatedAt)},
                    UpdatedAt = @{nameof(Resource.UpdatedAt)},
                    AccountId = @{nameof(Resource.AccountId)},
                    Path = @{nameof(Resource.Path)},
                    Size = @{nameof(Resource.Size)}
                WHERE 
                    Id = @{nameof(Resource.Id)}
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
