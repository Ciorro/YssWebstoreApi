using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Entities.Tags;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Tags.Queries
{
    public class GetVerifiedTagTreeQueryHandler
        : IQueryHandler<GetVerifiedTagTreeQuery, Result<string[]>>
    {
        private readonly IDbConnection _db;

        public GetVerifiedTagTreeQueryHandler(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<Result<string[]>> HandleAsync(GetVerifiedTagTreeQuery message, CancellationToken cancellationToken = default)
        {
            if (!Tag.TryParse(message.RootTag, out var rootTag))
            {
                return TagErrors.InvalidTagFormat;
            }

            var result = await _db.QueryAsync<string>(
                """
                WITH RECURSIVE TagTree AS (
                    SELECT Tags.Id FROM Tags WHERE Tag = @Root
                    UNION ALL
                    SELECT Tags.Id FROM Tags JOIN TagTree ON TagTree.Id = ANY(Tags.Parents)
                )
                SELECT Tags.Tag FROM Tags JOIN TagTree ON TagTree.Id = Tags.Id WHERE Tags.IsVerified
                """,
                new
                {
                    Root = message.RootTag
                });

            return result.ToArray();
        }
    }
}
