using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Posts;
using YssWebstoreApi.Models.DTOs.Projects;
using YssWebstoreApi.Persistance.Storage;

namespace YssWebstoreApi.Features.Posts.Queries
{
    public class GetPostByIdQueryHandler
        : IQueryHandler<GetPostByIdQuery, Result<PostResponse>>
    {
        private readonly IDbConnection _db;
        private readonly IFileStorage _fileStorage;

        public GetPostByIdQueryHandler(IDbConnection dbConnection, IFileStorage fileStorage)
        {
            _db = dbConnection;
            _fileStorage = fileStorage;
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
                    Accounts.UniqueName,
                    Accounts.DisplayName,
                    Accounts.StatusText,
                    Projects.Name,
                    Projects.Slug,
                    Resources.Path
                FROM 
                    Posts
                    JOIN Accounts ON Accounts.Id = Posts.AccountId
                    LEFT JOIN Projects ON Projects.Id = Posts.ProjectId
                    LEFT JOIN Resources ON Resources.Id = Posts.ImageId
                WHERE
                    Posts.Id = @PostId
                """,
                (post, account, project, imgPath) =>
                {
                    post.Account = account;
                    post.Project = project;

                    if (!string.IsNullOrEmpty(imgPath))
                    {
                        post.CoverImageUrl = _fileStorage.GetUrl(imgPath);
                    }

                    return post;
                },
                new
                {
                    message.PostId
                },
                splitOn: "Id,UniqueName,Name,Path");

            return posts.Any() ?
                posts.Single() : CommonErrors.ResourceNotFound;
        }
    }
}
