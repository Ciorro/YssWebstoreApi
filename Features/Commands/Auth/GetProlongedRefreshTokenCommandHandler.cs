using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Repositories.Abstractions;
using YssWebstoreApi.Security;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class GetProlongedRefreshTokenCommandHandler : IRequestHandler<GetProlongedRefreshTokenCommand, string?>
    {
        private readonly IDbConnection _cn;
        private readonly TimeProvider _timeProvider;

        public GetProlongedRefreshTokenCommandHandler(IDbConnection dbConnection, TimeProvider timeProvider)
        {
            _cn = dbConnection;
            _timeProvider = timeProvider;
        }

        public async Task<string?> Handle(GetProlongedRefreshTokenCommand request, CancellationToken cancellationToken)
        {

            var refreshToken = request.CurrentRefreshToken;
            var setsNewToken = string.IsNullOrEmpty(request.CurrentRefreshToken);
            var expiresAt = _timeProvider.GetUtcNow().Add(request.Length);

            if (setsNewToken)
            {
                refreshToken = SecurityUtils.GetRandomString(255);
            }

            var parameters = new
            {
                AccountId = request.AccountId,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = expiresAt
            };

            string sql = @"UPDATE credentials SET
                           RefreshToken=@RefreshToken,
                           RefreshTokenExpiresAt=@RefreshTokenExpiresAt
                           WHERE AccountId=@AccountId";

            if (!setsNewToken)
            {
                sql += " AND RefreshToken=@RefreshToken";
            }

            return await _cn.ExecuteAsync(sql,parameters) == 1 ?
                refreshToken : null;
        }
    }
}
