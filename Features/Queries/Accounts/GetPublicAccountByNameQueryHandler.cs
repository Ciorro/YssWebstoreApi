using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Features.Queries.Accounts
{
    public class GetPublicAccountByNameQueryHandler : IRequestHandler<GetPublicAccountByNameQuery, PublicAccount?>
    {
        private readonly IDbConnection _cn;

        public GetPublicAccountByNameQueryHandler(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<PublicAccount?> Handle(GetPublicAccountByNameQuery request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                UniqueName = request.UniqueName
            };

            string sql = @"SELECT accounts.Id,
                                  accounts.CreatedAt,
                                  accounts.UpdatedAt,
                                  accounts.UniqueName,
                                  accounts.DisplayName,
                                  accounts.Status, 
		                          COUNT(CASE WHEN friendships.FolloweeAccount=accounts.Id THEN 1 END) AS ""Followers"", 
 		                          COUNT(CASE WHEN friendships.FollowerAccount=accounts.Id THEN 1 END) AS ""Following""
                           FROM accounts, friendships
                           WHERE UniqueName=@UniqueName";

            return await _cn.QuerySingleOrDefaultAsync<PublicAccount>(sql, parameters);
        }
    }
}
