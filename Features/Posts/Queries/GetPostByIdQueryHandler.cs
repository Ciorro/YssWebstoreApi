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
            using var results = await _db.QueryMultipleAsync(
                """
                -- Select post
                SELECT
                    Posts.Id,
                    Posts.CreatedAt,
                    Posts.UpdatedAt,
                    Posts.Title,
                    Posts.Content,
                    Resources.Path AS CoverImageUrl
                FROM 
                    Posts 
                    LEFT JOIN Resources ON Resources.Id = Posts.ImageResourceId
                WHERE 
                    Posts.Id = @PostId;

                -- Select post author
                SELECT
                    Accounts.Id,
                    Accounts.UniqueName,
                    Accounts.DisplayName,
                    Accounts.StatusText,
                    Resources.Path AS AvatarUrl
                FROM 
                    Accounts 
                    JOIN Posts ON Posts.AccountId = Accounts.Id
                    LEFT JOIN Resources ON Resources.Id = Accounts.AvatarResourceId
                WHERE
                    Posts.Id = @PostId;

                -- Select post target project
                SELECT
                    Projects.Id,
                    Projects.Name,
                    Projects.Slug
                FROM 
                    Posts 
                    JOIN Projects ON Projects.Id = Posts.TargetProjectId
                WHERE
                    Posts.Id = @PostId;
                """, new { message.PostId });

            var post = await results.ReadSingleOrDefaultAsync<PostResponse>();
            if (post is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            post.Account = await results.ReadSingleAsync<AccountResponse>();
            post.Project = await results.ReadSingleOrDefaultAsync<ProjectLinkResponse>();

            post.CoverImageUrl = _storage.GetUrl(post.CoverImageUrl!);
            post.Account.AvatarUrl = _storage.GetUrl(post.Account.AvatarUrl!);

            return post;
        }
    }
}
