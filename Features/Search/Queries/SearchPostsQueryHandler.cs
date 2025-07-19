using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Posts;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Persistance.Storage;

namespace YssWebstoreApi.Features.Search.Queries
{
    public class SearchPostsQueryHandler
        : IQueryHandler<SearchPostsQuery, Result<Page<PostResponse>>>
    {
        private readonly IDbConnection _db;
        private readonly IFileStorage _fileStorage;

        public SearchPostsQueryHandler(IDbConnection dbConnection, IFileStorage fileStorage)
        {
            _db = dbConnection;
            _fileStorage = fileStorage;
        }

        public async Task<Result<Page<PostResponse>>> HandleAsync(SearchPostsQuery message, CancellationToken cancellationToken = default)
        {
            var allResultsIds = (await _db.QueryAsync<Guid>(message.GetCommandDefinition())).ToList();
            var limitedResultsIds = allResultsIds
                .Skip(message.PageOptions.GetOffset())
                .Take(message.PageOptions.PageSize);

            var results = await _db.QueryAsync<PostResponse, AccountResponse, ProjectLinkResponse, string, PostResponse>(
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
                    Accounts.Id,
                    Accounts.UniqueName,
                    Accounts.DisplayName,
                    Accounts.StatusText,
                    Projects.Id,
                    Projects.Name,
                    Projects.Slug,
                    Resources.Path
                FROM 
                    Posts
                    INNER JOIN Ids ON Ids.Id = Posts.Id
                    INNER JOIN Accounts ON Accounts.Id = Posts.AccountId
                    LEFT JOIN Projects ON Projects.Id = Posts.TargetProjectId
                    LEFT JOIN Resources ON Resources.Id = Posts.ImageResourceId
                ORDER BY
                    Ids.Order
                """,
                (post, account, project, imgPath) =>
                {
                    post.Account = account;
                    post.Project = project;

                    if (!string.IsNullOrWhiteSpace(imgPath))
                        post.CoverImageUrl = _fileStorage.GetUrl(imgPath);

                    return post;
                },
                new
                {
                    Ids = limitedResultsIds
                },
                splitOn: "Id,Path");

            return new Page<PostResponse>(
                pageNumber: message.PageOptions.PageNumber,
                pageSize: message.PageOptions.PageSize,
                totalCount: allResultsIds.Count,
                items: results.ToList());
        }
    }
}
