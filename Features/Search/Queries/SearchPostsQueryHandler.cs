using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Posts;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Search.Queries
{
    public class SearchPostsQueryHandler
        : IQueryHandler<SearchPostsQuery, ValueResult<Page<PostResponse>>>
    {
        private readonly IDbConnection _db;

        public SearchPostsQueryHandler(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<ValueResult<Page<PostResponse>>> HandleAsync(SearchPostsQuery message, CancellationToken cancellationToken = default)
        {
            var allResultsIds = (await _db.QueryAsync<Guid>(message.GetCommandDefinition())).ToList();
            var limitedResultsIds = allResultsIds
                .Skip(message.PageOptions.GetOffset())
                .Take(message.PageOptions.PageSize);

            var results = await _db.QueryAsync<PostResponse, AccountResponse, ProjectLinkResponse, PostResponse>(
                """
                WITH Ids AS (
                    SELECT
                        Id,
                        ROW_NUMBER() OVER() AS Order
                    FROM
                        UNNEST(@Ids) AS Id
                )
                SELECT
                    Posts.Id,
                    Posts.CreatedAt,
                    Posts.UpdatedAt,
                    Posts.Title,
                    Posts.Content,
                    Images.PublicUrl AS CoverImageUrl,
                    Accounts.Id,
                    Accounts.UniqueName,
                    Accounts.DisplayName,
                    Accounts.StatusText,
                    Avatar.PublicUrl AS AvatarUrl,
                    Projects.Id,
                    Projects.Name,
                    Projects.Slug,
                    ProjectIcons.PublicUrl AS IconUrl
                FROM 
                    Posts
                    INNER JOIN Ids ON Ids.Id = Posts.Id
                    INNER JOIN Accounts ON Accounts.Id = Posts.AccountId
                    LEFT JOIN Projects ON Projects.Id = Posts.TargetProjectId
                    LEFT JOIN Resources ProjectIcons ON ProjectIcons.Id = Projects.IconResourceId
                    LEFT JOIN Resources Images ON Images.Id = Posts.ImageResourceId
                    LEFT JOIN Resources Avatar ON Avatar.Id = Accounts.AvatarResourceId
                ORDER BY
                    Ids.Order
                """,
                (post, account, project) =>
                {
                    post.Account = account;
                    post.Project = project;

                    return post;
                },
                new
                {
                    Ids = limitedResultsIds
                });

            return new Page<PostResponse>(
                pageNumber: message.PageOptions.PageNumber,
                pageSize: message.PageOptions.PageSize,
                totalCount: allResultsIds.Count,
                items: results.ToList());
        }
    }
}
