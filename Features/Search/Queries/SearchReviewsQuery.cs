using Dapper;
using YssWebstoreApi.Api.DTO.Reviews;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Search.Queries
{
    public class SearchReviewsQuery : SearchQuery<ValueResult<Page<ReviewResponse>>>
    {
        public Guid? AccountId { get; set; }
        public Guid? ProjectId { get; set; }

        public override CommandDefinition GetCommandDefinition()
        {
            var builder = new SqlBuilder();
            var template = builder.AddTemplate(
                """
                SELECT
                	Reviews.Id
                FROM 
                	Reviews 
                    JOIN Accounts ON Accounts.Id = Reviews.AccountId
                /**where**/
                /**orderby**/
                """);

            if (AccountId.HasValue)
                builder.Where("Reviews.AccountId = @AccountId", new { AccountId });

            if (ProjectId.HasValue)
                builder.Where("Reviews.ProjectId = @ProjectId", new { ProjectId });

            switch (SortOptions.OrderBy?.ToLower())
            {
                case "created":
                    builder.OrderBy($"Reviews.CreatedAt {SortOptions.Order}");
                    break;
                case "rate":
                    builder.OrderBy($"Reviews.Rate {SortOptions.Order}");
                    break;
            }

            return new CommandDefinition(template.RawSql, template.Parameters);
        }
    }
}
