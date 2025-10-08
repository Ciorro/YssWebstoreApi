using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Reviews;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Reviews.Queries
{
    public class GetReviewsSummaryQueryHandler
        : IQueryHandler<GetReviewsSummaryQuery, Result<ReviewsSummaryResponse>>
    {
        private readonly IDbConnection _db;

        public GetReviewsSummaryQueryHandler(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<Result<ReviewsSummaryResponse>> HandleAsync(GetReviewsSummaryQuery message, CancellationToken cancellationToken = default)
        {
            var rateCounts = await _db.QueryAsync<(int rate, int count)>(
                """
                SELECT
                    Rate,
                    COUNT(*) AS Count
                FROM 
                    Reviews
                WHERE
                    Reviews.ProjectId = @ProjectId
                GROUP BY
                    Reviews.Rate
                """,
                new
                {
                    message.ProjectId
                });

            var reviewsSummary = new ReviewsSummaryResponse(1, 5);
            foreach (var rateCount in rateCounts)
            {
                reviewsSummary.AddRateCounts(rateCount.rate, rateCount.count);
            }

            return reviewsSummary;
        }
    }
}
