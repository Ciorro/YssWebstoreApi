using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Models.DTOs.Review;

namespace YssWebstoreApi.Features.Queries.Reviews
{
    public class GetReviewsSummaryByProductIdQueryHandler : IRequestHandler<GetReviewsSummaryByProductIdQuery, ReviewsSummary?>
    {
        private readonly IDbConnection _cn;

        public GetReviewsSummaryByProductIdQueryHandler(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<ReviewsSummary?> Handle(GetReviewsSummaryByProductIdQuery request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                ProductId = request.ProductId,
            };

            string sql = @"SELECT Rate,
                                  COUNT(*) AS RateCount
                           FROM reviews
                           WHERE reviews.ProductId=1
                           GROUP BY rate";

            return (await _cn.QueryAsync<(int rate, int count)>(sql, parameters))
                .Aggregate(new ReviewsSummary(), (total, next) =>
                {
                    total.AddRates(next.rate, next.count);
                    return total;
                });
        }
    }
}
