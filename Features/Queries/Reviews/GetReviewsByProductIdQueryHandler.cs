using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Review;

namespace YssWebstoreApi.Features.Queries.Reviews
{
    public class GetReviewsByProductIdQueryHandler : IRequestHandler<GetReviewsByProductIdQuery, IList<PublicReview>>
    {
        private readonly IDbConnection _cn;

        public GetReviewsByProductIdQueryHandler(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<IList<PublicReview>> Handle(GetReviewsByProductIdQuery request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                ProductId = request.ProductId
            };

            string sql = @"SELECT reviews.*,
                                  accounts.*
                           FROM reviews
                           LEFT JOIN accounts ON reviews.AccountId=accounts.Id
                           WHERE reviews.ProductId = @ProductId";

            var reviews = await _cn.QueryAsync<PublicReview, PublicAccount, PublicReview>(sql, (review, account) =>
            {
                review.Account = account;
                return review;
            }, parameters);

            return reviews.ToList();
        }
    }
}
