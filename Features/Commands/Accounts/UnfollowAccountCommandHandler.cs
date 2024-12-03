using Dapper;
using MediatR;
using System.Data;

namespace YssWebstoreApi.Features.Commands.Accounts
{
    public class UnfollowAccountCommandHandler : IRequestHandler<UnfollowAccountCommand, Unit>
    {
        private readonly IDbConnection _cn;

        public UnfollowAccountCommandHandler(IDbConnection connection)
        {
            _cn = connection;
        }

        public async Task<Unit> Handle(UnfollowAccountCommand request, CancellationToken cancellationToken)
        {
            string sql = @"DELETE FROM friendships 
                           WHERE FollowerAccount=@FollowerId AND
                                 FolloweeAccount=(SELECT Id FROM accounts WHERE UniqueName=@FolloweeName)";

            await _cn.ExecuteAsync(sql, request);
            return Unit.Value;
        }
    }
}
