using Dapper;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Search.Queries
{
    public class SearchAccountsQuery : SearchQuery<ValueResult<Page<AccountResponse>>>
    {
        public Guid? FollowedBy { get; set; }
        public Guid? Following { get; set; }

        public override CommandDefinition GetCommandDefinition()
        {
            var builder = new SqlBuilder();
            var template = builder.AddTemplate(
                """
                SELECT
                   Accounts.Id
                FROM
                   Accounts
                   LEFT JOIN AccountFollows ON AccountFollows.FollowerId = Accounts.Id
                                            OR AccountFollows.FollowedId = Accounts.Id
                /**where**/
                GROUP BY
                   Accounts.Id,
                   AccountFollows.Id
                /**orderby**/
                """);

            if (!string.IsNullOrWhiteSpace(SearchText))
                builder.Where(
                    """
                    Accounts.UniqueName LIKE @SearchText OR Accounts.DisplayName LIKE @SearchText
                    """, new { SearchText = $"%{SearchText}%" });

            // TODO: Including followedby and following in the search query doesn't work. (the '<>' conditions makes them exclude each other)
            if (FollowedBy.HasValue)
                builder.Where(
                    """
                    AccountFollows.FollowerId = @FollowedBy AND AccountFollows.FollowerId <> Accounts.Id
                    """, new { FollowedBy });

            if (Following.HasValue)
                builder.Where(
                    """
                    AccountFollows.FollowedId = @Following AND AccountFollows.FollowedId <> Accounts.Id
                    """, new { Following });

            switch (SortOptions.OrderBy?.ToLower())
            {
                case "followedat":
                    builder.OrderBy($"AccountFollows.CreatedAt {SortOptions.Order}");
                    break;
                case "name":
                    builder.OrderBy($"Accounts.DisplayName {SortOptions.Order}");
                    break;
            }

            return new CommandDefinition(template.RawSql, template.Parameters);
        }
    }
}
