using Dapper;
using LiteBus.Commands.Abstractions;
using System.Data;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class FollowAccountCommandHandler
        : ICommandHandler<FollowAccountCommand, Result>
    {
        private readonly IDbConnection _db;
        private readonly TimeProvider _timeProvider;

        public FollowAccountCommandHandler(IDbConnection dbConnection, TimeProvider timeProvider)
        {
            _db = dbConnection;
            _timeProvider = timeProvider;
        }

        public async Task<Result> HandleAsync(FollowAccountCommand message, CancellationToken cancellationToken = default)
        {
            if (message.FollowerId == message.FollowedId)
            {
                return AccountErrors.FollowedSelf;
            }

            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;
            var id = Guid.CreateVersion7(creationTime);

            await _db.ExecuteAsync(
                """
                INSERT INTO AccountFollows (
                    Id,
                    CreatedAt,
                    FollowerId,
                    FollowedId
                ) VALUES (
                    @Id,
                    @CreatedAt,
                    @FollowerId,
                    @FollowedId
                )
                """,
                new
                {
                    Id = id,
                    CreatedAt = creationTime,
                    message.FollowerId,
                    message.FollowedId
                });

            return Result.Ok();
        }
    }
}
