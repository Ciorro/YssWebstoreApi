using Dapper;
using MediatR;
using System.Data;

namespace YssWebstoreApi.Features.Commands.Accounts
{
    public class FollowAccountCommandHandler : IRequestHandler<FollowAccountCommand, Unit>
    {
        private readonly IDbConnection _cn;

        public FollowAccountCommandHandler(IDbConnection connection)
        {
            _cn = connection;
        }

        public async Task<Unit> Handle(FollowAccountCommand request, CancellationToken cancellationToken)
        {
            ulong? followeeId = await _cn.QuerySingleOrDefaultAsync<ulong?>(
                "SELECT Id FROM accounts WHERE UniqueName=@FolloweeName", request);

            if (followeeId != request.FollowerId && followeeId.HasValue)
            {
                var parameters = new
                {
                    FollowerId = request.FollowerId,
                    FolloweeId = followeeId
                };

                string sql = @"INSERT INTO friendships (FollowerAccount,FolloweeAccount) 
                               VALUES (@FollowerId, @FolloweeId)";

                await _cn.ExecuteAsync(sql, parameters);
            }

            return Unit.Value;
        }
    }
}
