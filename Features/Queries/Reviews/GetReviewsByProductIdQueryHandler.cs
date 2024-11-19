using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Models.Api;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Review;
using YssWebstoreApi.Models.Query;

namespace YssWebstoreApi.Features.Queries.Reviews
{
    public class GetReviewsByProductIdQueryHandler : IRequestHandler<GetReviewsByProductIdQuery, Page<PublicReview>>
    {
        private readonly IDbConnection _cn;

        public GetReviewsByProductIdQueryHandler(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<Page<PublicReview>> Handle(GetReviewsByProductIdQuery request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                ProductId = request.ProductId
            };

            string sql = @"SELECT reviews.*,
                                  accounts.*
                           FROM reviews
                           LEFT JOIN accounts ON reviews.AccountId=accounts.Id
                           WHERE reviews.ProductId = @ProductId
                           ORDER BY reviews.CreatedAt DESC";

            var reviews = await _cn.QueryAsync<PublicReview, PublicAccount, PublicReview>(sql, (review, account) =>
            {
                review.Account = account;
                return review;
            }, parameters);

            var pageOptions = request.PageOptions;
            var reviewCount = reviews.Count();
            var pageSize = pageOptions.PageSize ?? reviewCount;

            return new Page<PublicReview>
            {
                PageNumber = pageOptions.PageNumber,
                PageSize = pageSize,
                ItemCount = reviews.Count(),
                Items = reviews.Skip(pageOptions.PageNumber * pageSize)
                               .Take(pageSize)
                               .ToArray()
            };
        }
    }
}
