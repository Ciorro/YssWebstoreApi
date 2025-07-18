using Dapper;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Features.Search.Queries
{
    public class SearchProjectsQuery : SearchQuery<Result<Page<ProjectSearchResult>>>
    {
        public string? AccountName { get; init; }
        public TagCollection? Tags { get; init; }
        public DateTime? MinReleasedAt { get; init; }
        public DateTime? MaxReleasedAt { get; init; }
        public bool PinnedOnly { get; init; }

        public override CommandDefinition GetCommandDefinition()
        {
            var builder = new SqlBuilder();
            var template = builder.AddTemplate(
                """
                SELECT
                	Projects.Id
                FROM 
                	Projects
                	JOIN Accounts ON Accounts.Id = Projects.AccountId
                	LEFT JOIN Reviews ON Reviews.ProjectId = Projects.Id
                	LEFT JOIN ProjectTags ON ProjectTags.ProjectId = Projects.Id
                	LEFT JOIN Tags ON Tags.Id = ProjectTags.TagId
                /**where**/
                GROUP BY
                	Projects.Id
                /**having**/
                /**orderby**/
                """);

            if (!string.IsNullOrWhiteSpace(SearchText))
                builder.Where("Projects.Name LIKE @SearchText", new { SearchText = $"%{SearchText}%" });

            if (!string.IsNullOrWhiteSpace(AccountName))
                builder.Where("Accounts.UniqueName = @AccountName", new { AccountName });

            if (MinReleasedAt.HasValue)
                builder.Where("Projects.ReleasedAt >= @MinReleasedAt", new { MinReleasedAt });

            if (MaxReleasedAt.HasValue)
                builder.Where("Projects.ReleasedAt >= @MaxReleasedAt", new { MaxReleasedAt });

            if (PinnedOnly)
                builder.Where("Projects.IsPinned = TRUE");

            if (Tags?.Any() == true)
                builder.Having("ARRAY_AGG(Tags.Tag) @> @Tags", new { Tags = Tags.Select(x => x.ToString()).ToList() });

            switch (SortOptions.OrderBy?.ToLower())
            {
                case "releasedate":
                    builder.OrderBy($"Projects.ReleasedAt {SortOptions.Order}");
                    break;
                case "title":
                    builder.OrderBy($"Projects.Name {SortOptions.Order}");
                    break;
            }

            return new CommandDefinition(template.RawSql, template.Parameters);
        }
    }
}
