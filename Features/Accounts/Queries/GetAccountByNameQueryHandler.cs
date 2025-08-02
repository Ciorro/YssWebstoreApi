using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Queries
{
    public class GetAccountByNameQueryHandler
        : IQueryHandler<GetAccountByNameQuery, Result<AccountResponse>>
    {
        private readonly IDbConnection _db;
        private readonly IStorage _storage;

        public GetAccountByNameQueryHandler(IDbConnection dbConnection, IStorage storage)
        {
            _db = dbConnection;
            _storage = storage;
        }

        public async Task<Result<AccountResponse>> HandleAsync(GetAccountByNameQuery message, CancellationToken cancellationToken = default)
        {
            var account = await _db.QuerySingleOrDefaultAsync<AccountResponse>(
                """
                SELECT
                    Accounts.Id,
                    Accounts.CreatedAt,
                    Accounts.UpdatedAt,
                    Accounts.UniqueName,
                    Accounts.DisplayName,
                    Accounts.StatusText,
                    Resources.Path AS AvatarUrl,
                    COUNT(CASE WHEN AccountFollows.FollowerId = Accounts.Id THEN 1 END) AS FollowingCount,
                    COUNT(CASE WHEN AccountFollows.FollowedId = Accounts.Id THEN 1 END) AS FollowersCount
                FROM
                    Accounts
                    LEFT JOIN AccountFollows ON AccountFollows.FollowerId = Accounts.Id
                                             OR AccountFollows.FollowedId = Accounts.Id
                    LEFT JOIN Resources ON Resources.Id = Accounts.AvatarResourceId
                WHERE
                    Accounts.UniqueName = @UniqueName
                GROUP BY
                    Accounts.Id,
                    Resources.Id
                """,
                new
                {
                    message.UniqueName
                });

            if (account is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            if (!string.IsNullOrEmpty(account.AvatarUrl))
            {
                account.AvatarUrl = _storage.GetUrl(account.AvatarUrl);
            }

            return account;
        }
    }
}
