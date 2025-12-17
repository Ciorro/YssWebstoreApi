using Dapper;
using LiteBus.Commands.Abstractions;
using LiteBus.Messaging.Abstractions;
using System.Data;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class SetProjectPinnedCommandValidator
        : ICommandValidator<SetProjectPinnedCommand>
    {
        const int MAX_PINS = 6;

        private readonly IDbConnection _db;

        public SetProjectPinnedCommandValidator(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task ValidateAsync(SetProjectPinnedCommand command, CancellationToken cancellationToken = default)
        {
            if (!command.IsPinned)
            {
                return;
            }

            int pinnedCount = await _db.QuerySingleAsync<int>(
                """
                SELECT COUNT(*) FROM Projects 
                WHERE Projects.AccountId = @AccountId 
                  AND Projects.IsPinned = TRUE
                """,
                new { command.AccountId });

            if (pinnedCount >= MAX_PINS)
            {
                AmbientExecutionContext.Current.Abort(Result.Fail(ProjectErrors.PinnedProjectLimit));
            }
        }
    }
}
