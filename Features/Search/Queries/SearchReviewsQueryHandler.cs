using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Reviews;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Search.Queries
{
    public class SearchReviewsQueryHandler
        : IQueryHandler<SearchReviewsQuery, ValueResult<Page<ReviewResponse>>>
    {
        private readonly IDbConnection _db;

        public SearchReviewsQueryHandler(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<ValueResult<Page<ReviewResponse>>> HandleAsync(SearchReviewsQuery message, CancellationToken cancellationToken = default)
        {
            var allResultsIds = (await _db.QueryAsync<Guid>(message.GetCommandDefinition())).ToList();
            var limitedResultsIds = allResultsIds
                .Skip(message.PageOptions.GetOffset())
                .Take(message.PageOptions.PageSize);

            var results = await _db.QueryAsync<ReviewResponse, AccountResponse, ReviewResponse>(
                """
                WITH Ids AS (
                    SELECT
                        Id,
                        ROW_NUMBER() OVER() AS Order
                    FROM
                        UNNEST(@Ids) AS Id
                )
                SELECT
                    Reviews.Id,
                    Reviews.CreatedAt,
                    Reviews.UpdatedAt,
                    Reviews.Rate,
                    Reviews.Content,
                    Accounts.Id,
                    Accounts.UniqueName,
                    Accounts.DisplayName,
                    Accounts.StatusText,
                    Resources.PublicUrl AS AvatarUrl
                FROM
                    Reviews
                    INNER JOIN Ids ON Ids.Id = Reviews.Id
                    INNER JOIN Accounts ON Accounts.Id = Reviews.AccountId
                    LEFT JOIN Resources ON Resources.Id = Accounts.AvatarResourceId
                ORDER BY
                    Ids.Order
                """,
                (review, account) =>
                {
                    review.Account = account;
                    return review;
                },
                new
                {
                    Ids = limitedResultsIds
                });


            return new Page<ReviewResponse>(
                pageNumber: message.PageOptions.PageNumber,
                pageSize: message.PageOptions.PageSize,
                totalCount: allResultsIds.Count,
                items: results.ToList());
        }
    }
}
