using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Search.Queries
{
    public class SearchAccountsQueryHandler
        : IQueryHandler<SearchAccountsQuery, Result<Page<AccountResponse>>>
    {
        private readonly IDbConnection _db;
        private readonly IStorage _storage;

        public SearchAccountsQueryHandler(IDbConnection dbConnection, IStorage storage)
        {
            _db = dbConnection;
            _storage = storage;
        }

        public async Task<Result<Page<AccountResponse>>> HandleAsync(SearchAccountsQuery message, CancellationToken cancellationToken = default)
        {
            var allResultsIds = (await _db.QueryAsync<Guid>(message.GetCommandDefinition())).ToList();
            var limitedResultsIds = allResultsIds
                .Skip(message.PageOptions.GetOffset())
                .Take(message.PageOptions.PageSize);

            var results = await _db.QueryAsync<AccountResponse>(
                """
                WITH Ids AS (
                    SELECT
                        Id,
                        ROW_NUMBER() OVER() AS Order
                    FROM
                        UNNEST(@Ids) AS Id
                )
                SELECT
                    Accounts.Id,
                    Accounts.CreatedAt,
                    Accounts.UpdatedAt,
                    Accounts.UniqueName,
                    Accounts.DisplayName,
                    Accounts.StatusText,
                    Resources.Path AS AvatarUrl
                FROM
                    Accounts 
                    INNER JOIN Ids ON Ids.Id = Accounts.Id
                    LEFT JOIN Resources ON Resources.Id = Accounts.AvatarResourceId
                ORDER BY
                    Ids.Order
                """,
                new
                {
                    Ids = limitedResultsIds
                });

            foreach (var account in results)
            {
                account.AvatarUrl = _storage.GetUrl(account.AvatarUrl!);
            }

            return new Page<AccountResponse>(
                pageNumber: message.PageOptions.PageNumber,
                pageSize: message.PageOptions.PageSize,
                totalCount: allResultsIds.Count,
                items: results.ToList());
        }
    }
}
