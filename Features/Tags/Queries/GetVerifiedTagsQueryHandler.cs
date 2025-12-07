using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Tags.Queries
{
    public class GetVerifiedTagsQueryHandler
        : IQueryHandler<GetVerifiedTagsQuery, ValueResult<string[]>>
    {
        private readonly IDbConnection _db;

        public GetVerifiedTagsQueryHandler(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<ValueResult<string[]>> HandleAsync(GetVerifiedTagsQuery message, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(message.Group) && !IsValidTagGroup(message.Group))
            {
                return TagErrors.InvalidGroupName;
            }

            var builder = new SqlBuilder();
            var template = builder.AddTemplate(
                """
                SELECT Tag FROM Tags /**where**/
                """);

            builder.Where("IsVerified=TRUE");
            builder.Where("Tag ILIKE @Search",
                new
                {
                    Search = $"{message.Group ?? "%"}-%{message.SearchText}%"
                });


            return (await _db.QueryAsync<string>(template.RawSql, template.Parameters))
                .ToArray();
        }

        private bool IsValidTagGroup(string group)
        {
            return group.All(char.IsAsciiLetter);
        }
    }
}
