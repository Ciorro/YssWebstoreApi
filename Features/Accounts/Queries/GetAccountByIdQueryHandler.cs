using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Queries
{
    public class GetAccountByIdQueryHandler
        : IQueryHandler<GetAccountByIdQuery, ValueResult<AccountResponse>>
    {
        private readonly IDbConnection _db;

        public GetAccountByIdQueryHandler(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<ValueResult<AccountResponse>> HandleAsync(GetAccountByIdQuery message, CancellationToken cancellationToken = default)
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
                    Resources.PublicUrl AS AvatarUrl,
                    CASE WHEN IsFollowed IS NOT NULL THEN TRUE ELSE FALSE END AS IsFollowed,
                    COUNT(CASE WHEN AccountFollows.FollowerId = Accounts.Id THEN 1 END) AS FollowingCount,
                    COUNT(CASE WHEN AccountFollows.FollowedId = Accounts.Id THEN 1 END) AS FollowersCount
                FROM
                    Accounts
                    LEFT JOIN AccountFollows ON AccountFollows.FollowerId = Accounts.Id
                                             OR AccountFollows.FollowedId = Accounts.Id
                    LEFT JOIN AccountFollows AS IsFollowed ON IsFollowed.FollowerId = @FollowedBy
                                                           AND IsFollowed.FollowedId = Accounts.Id
                    LEFT JOIN Resources ON Resources.Id = Accounts.AvatarResourceId
                WHERE
                    Accounts.Id = @AccountId
                GROUP BY
                    Accounts.Id,
                    Resources.Id,
                    IsFollowed
                """,
                new
                {
                    message.AccountId,
                    message.FollowedBy
                });

            if (account is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            return account;
        }
    }
}
