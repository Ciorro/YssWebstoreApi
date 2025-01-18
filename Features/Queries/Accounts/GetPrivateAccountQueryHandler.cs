using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Features.Queries.Accounts
{
    public class GetPrivateAccountQueryHandler : IRequestHandler<GetPrivateAccountQuery, PrivateAccount?>
    {
        private readonly IDbConnection _cn;

        public GetPrivateAccountQueryHandler(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<PrivateAccount?> Handle(GetPrivateAccountQuery request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                Id = request.AccountId
            };

            string sql = @"SELECT accounts.Id,
                                  accounts.CreatedAt,
                                  accounts.UpdatedAt,
                                  accounts.UniqueName,
                                  accounts.DisplayName,
                                  accounts.Status,
                                  credentials.Email, 
		                          COUNT(CASE WHEN friendships.FolloweeAccount=accounts.Id THEN 1 END) AS ""Followers"", 
 		                          COUNT(CASE WHEN friendships.FollowerAccount=accounts.Id THEN 1 END) AS ""Following""
                           FROM accounts, credentials, friendships
                           WHERE accounts.Id=credentials.AccountId AND accounts.Id=@Id
                           GROUP BY accounts.Id";

            return await _cn.QuerySingleOrDefaultAsync<PrivateAccount>(sql, parameters);
        }
    }
}
