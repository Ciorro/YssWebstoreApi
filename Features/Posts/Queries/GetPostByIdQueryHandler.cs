using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Posts;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Queries
{
    public class GetPostByIdQueryHandler
        : IQueryHandler<GetPostByIdQuery, Result<PostResponse>>
    {
        private readonly IDbConnection _db;
        private readonly IStorage _storage;

        public GetPostByIdQueryHandler(IDbConnection dbConnection, IStorage storage)
        {
            _db = dbConnection;
            _storage = storage;
        }

        public async Task<Result<PostResponse>> HandleAsync(GetPostByIdQuery message, CancellationToken cancellationToken = default)
        {
            var posts = await _db.QueryAsync<PostResponse, AccountResponse, ProjectLinkResponse, string, PostResponse>(
                """
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
                    JOIN Accounts ON Accounts.Id = Posts.AccountId
                    LEFT JOIN Projects ON Projects.Id = Posts.TargetProjectId
                    LEFT JOIN Resources ON Resources.Id = Posts.ImageResourceId
                WHERE
                    Posts.Id = @PostId
                """,
                (post, account, project, imgPath) =>
                {
                    post.Account = account;
                    post.Project = project;
                    post.CoverImageUrl = _storage.GetUrl(imgPath);

                    return post;
                },
                new
                {
                    message.PostId
                },
                splitOn: "Id,Path");

            return posts.Any() ?
                posts.Single() : CommonErrors.ResourceNotFound;
        }
    }
}
