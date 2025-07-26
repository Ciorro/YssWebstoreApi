using Dapper;
using System.Data;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Persistance.Repositories
{
    public class ReviewRepository : IRepository<Review>
    {
        private readonly IDbConnection _db;

        public ReviewRepository(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<Review?> GetAsync(Guid id)
        {
            return await _db.QuerySingleOrDefaultAsync<Review>(
                """
                SELECT
                    Reviews.Id,
                    Reviews.CreatedAt,
                    Reviews.UpdatedAt,
                    Reviews.AccountId,
                    Reviews.ProjectId,
                    Reviews.Rate,
                    Reviews.Content
                FROM Reviews WHERE Reviews.Id = @Id
                """,
                new
                {
                    Id = id
                });
        }

        public async Task InsertAsync(Review entity)
        {
            await _db.ExecuteAsync(
                $"""
                INSERT INTO Reviews (
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    AccountId,
                    ProjectId,
                    Rate,
                    Content
                ) VALUES (
                    @{nameof(Review.Id)},
                    @{nameof(Review.CreatedAt)},
                    @{nameof(Review.UpdatedAt)},
                    @{nameof(Review.AccountId)},
                    @{nameof(Review.ProjectId)},
                    @{nameof(Review.Rate)},
                    @{nameof(Review.Content)}                
                )
                """, entity);
        }

        public async Task UpdateAsync(Review entity)
        {
            await _db.ExecuteAsync(
                $"""
                UPDATE Reviews
                SET Id = @{nameof(Review.Id)}
                    CreatedAt = @{nameof(Review.CreatedAt)},
                    UpdatedAt = @{nameof(Review.UpdatedAt)},
                    AccountId = @{nameof(Review.AccountId)},
                    ProjectId = @{nameof(Review.ProjectId)},
                    Rate = @{nameof(Review.Rate)},
                    Content = @{nameof(Review.Content)}  
                WHERE Reviews.Id = @Id
                """, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.ExecuteAsync(
                """
                DELETE FROM Reviews WHERE Reviews.Id = @Id
                """,
                new
                {
                    Id = id
                });
        }
    }
}
