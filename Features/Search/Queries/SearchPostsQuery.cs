using Dapper;
using YssWebstoreApi.Api.DTO.Posts;
using YssWebstoreApi.Api.DTO.Search;

namespace YssWebstoreApi.Features.Search.Queries
{
    public class SearchPostsQuery : SearchQuery<Result<Page<PostResponse>>>
    {
        public string? AccountName { get; init; }
        public Guid? TargetProjectId { get; init; }

        public override CommandDefinition GetCommandDefinition()
        {
            var builder = new SqlBuilder();
            var template = builder.AddTemplate(
                """
                SELECT
                	Posts.Id
                FROM 
                	Posts
                	JOIN Accounts ON Accounts.Id = Posts.AccountId
                /**where**/
                GROUP BY
                	Posts.Id
                /**orderby**/
                """);

            if (!string.IsNullOrWhiteSpace(SearchText))
                builder.Where("Posts.Title LIKE @SearchText", new { SearchText = $"%{SearchText}%" });

            if (!string.IsNullOrWhiteSpace(AccountName))
                builder.Where("Accounts.UniqueName = @AccountName", new { AccountName });

            if (TargetProjectId.HasValue)
                builder.Where("Posts.TargetProjectId = @TargetProjectId", new { TargetProjectId });

            switch (SortOptions.OrderBy?.ToLower())
            {
                case "createdate":
                    builder.OrderBy($"Posts.CreatedAt {SortOptions.Order}");
                    break;
            }

            return new CommandDefinition(template.RawSql, template.Parameters);
        }
    }
}
