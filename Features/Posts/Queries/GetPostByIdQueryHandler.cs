using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Posts;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Queries
{
    public class GetPostByIdQueryHandler
        : IQueryHandler<GetPostByIdQuery, ValueResult<PostResponse>>
    {
        private readonly IDbConnection _db;

        public GetPostByIdQueryHandler(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<ValueResult<PostResponse>> HandleAsync(GetPostByIdQuery message, CancellationToken cancellationToken = default)
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
                    Resources.PublicUrl AS CoverImageUrl
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
                    Resources.PublicUrl AS AvatarUrl
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
                    Projects.Slug,
                    Resources.PublicUrl AS IconUrl
                FROM 
                    Posts 
                    JOIN Projects ON Projects.Id = Posts.TargetProjectId
                    LEFT JOIN Resources ON Resources.Id = Projects.IconResourceId
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

            return post;
        }
    }
}
