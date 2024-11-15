using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Features.Queries.Accounts
{
    public class GetPublicAccountByIdQueryHandler : IRequestHandler<GetPublicAccountByIdQuery, PublicAccount?>
    {
        private readonly IDbConnection _cn;

        public GetPublicAccountByIdQueryHandler(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<PublicAccount?> Handle(GetPublicAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                Id = request.AccountId
            };

            string sql = @"SELECT accounts.Id,
                                  accounts.CreatedAt,
                                  accounts.UniqueName,
                                  accounts.DisplayName,
                                  accounts.Status
                           FROM accounts
                           WHERE Id=@Id";

            return await _cn.QuerySingleOrDefaultAsync<PublicAccount>(sql, parameters);
        }
    }
}
