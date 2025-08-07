using Dapper;
using LiteBus.Commands.Abstractions;
using System.Data;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class UnfollowAccountCommandHandler
        : ICommandHandler<UnfollowAccountCommand, Result>
    {
        private readonly IDbConnection _db;

        public UnfollowAccountCommandHandler(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<Result> HandleAsync(UnfollowAccountCommand message, CancellationToken cancellationToken = default)
        {
            await _db.ExecuteAsync(
                """
                DELETE FROM AccountFollows
                WHERE FollowerId = @FollowerId 
                  AND FollowedId = @FollowedId
                """,
                new
                {
                    message.FollowerId,
                    message.FollowedId
                });

            return Result.Ok();
        }
    }
}
