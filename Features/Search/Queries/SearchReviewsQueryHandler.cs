using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Api.DTO.Reviews;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Search.Queries
{
    public class SearchReviewsQueryHandler
        : IQueryHandler<SearchReviewsQuery, Result<Page<ReviewResponse>>>
    {
        private readonly IDbConnection _db;
        private readonly IStorage _storage;

        public SearchReviewsQueryHandler(IDbConnection dbConnection, IStorage storage)
        {
            _db = dbConnection;
            _storage = storage;
        }

        public async Task<Result<Page<ReviewResponse>>> HandleAsync(SearchReviewsQuery message, CancellationToken cancellationToken = default)
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
                    Resources.Path AS AvatarUrl
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
                    review.Account.AvatarUrl = _storage.GetUrl(review.Account.AvatarUrl!);
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
