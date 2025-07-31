using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Posts;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Search.Queries
{
    public class SearchPostsQueryHandler
        : IQueryHandler<SearchPostsQuery, Result<Page<PostResponse>>>
    {
        private readonly IDbConnection _db;
        private readonly IStorage _storage;

        public SearchPostsQueryHandler(IDbConnection dbConnection, IStorage storage)
        {
            _db = dbConnection;
            _storage = storage;
        }

        public async Task<Result<Page<PostResponse>>> HandleAsync(SearchPostsQuery message, CancellationToken cancellationToken = default)
        {
            var allResultsIds = (await _db.QueryAsync<Guid>(message.GetCommandDefinition())).ToList();
            var limitedResultsIds = allResultsIds
                .Skip(message.PageOptions.GetOffset())
                .Take(message.PageOptions.PageSize);

            var results = await _db.QueryAsync<PostResponse, AccountResponse, ProjectLinkResponse, string, string, PostResponse>(
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
                    Images.Path,
                    Avatar.Path
                FROM 
                    Posts
                    INNER JOIN Ids ON Ids.Id = Posts.Id
                    INNER JOIN Accounts ON Accounts.Id = Posts.AccountId
                    LEFT JOIN Projects ON Projects.Id = Posts.TargetProjectId
                    LEFT JOIN Resources Images ON Images.Id = Posts.ImageResourceId
                    LEFT JOIN Resources Avatar ON Avatar.Id = Accounts.AvatarResourceId
                ORDER BY
                    Ids.Order
                """,
                (post, account, project, imgPath, avatarPath) =>
                {
                    post.Account = account;
                    post.Account.AvatarUrl = _storage.GetUrl(avatarPath);
                    post.Project = project;
                    post.CoverImageUrl = _storage.GetUrl(imgPath);

                    return post;
                },
                new
                {
                    Ids = limitedResultsIds
                },
                splitOn: "Id,,Id,Id,Path,Path");

            return new Page<PostResponse>(
                pageNumber: message.PageOptions.PageNumber,
                pageSize: message.PageOptions.PageSize,
                totalCount: allResultsIds.Count,
                items: results.ToList());
        }
    }
}
