using Dapper;
using LiteBus.Commands.Abstractions;
using System.Data;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class CreateProjectCommandPreHandler
        : ICommandPreHandler<CreateProjectCommand>
    {
        private readonly IDbConnection _db;
        private readonly TimeProvider _timeProvider;

        public CreateProjectCommandPreHandler(IDbConnection dbConnection, TimeProvider timeProvider)
        {
            _db = dbConnection;
            _timeProvider = timeProvider;
        }

        public Task PreHandleAsync(CreateProjectCommand message, CancellationToken cancellationToken = default)
        {
            return EnsureTagsExist(message);
        }

        private async Task EnsureTagsExist(CreateProjectCommand message)
        {
            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;
            var tags = message.Tags.GetGroup("tag");

            await _db.ExecuteAsync(
                """
                INSERT INTO Tags (
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    Tag
                ) VALUES (
                    @Id,
                    @CreatedAt,
                    @UpdatedAt,
                    @Tag
                ) ON CONFLICT (Tag) DO NOTHING;
                """,
                tags.Select(
                    x => new
                    {
                        Id = Guid.CreateVersion7(creationTime),
                        CreatedAt = creationTime,
                        UpdatedAt = creationTime,
                        Tag = x.ToString()
                    }));
        }
    }
}
