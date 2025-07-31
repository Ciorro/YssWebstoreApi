using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Queries
{
    public class GetProjectBySlugQueryHandler
        : IQueryHandler<GetProjectBySlugQuery, Result<ProjectResponse>>
    {
        private readonly IDbConnection _db;
        private readonly IStorage _storage;

        public GetProjectBySlugQueryHandler(IDbConnection dbConnection, IStorage storage)
        {
            _db = dbConnection;
            _storage = storage;
        }

        public async Task<Result<ProjectResponse>> HandleAsync(GetProjectBySlugQuery message, CancellationToken cancellationToken = default)
        {
            using var results = await _db.QueryMultipleAsync(
                """
                -- Select project
                SELECT
                    Projects.Id,
                	Projects.CreatedAt,
                	Projects.UpdatedAt,
                	Projects.ReleasedAt,
                	Projects.Name,
                	Projects.Slug,
                	Projects.Description
                FROM Projects WHERE Projects.Slug = @Slug;

                -- Select account
                SELECT
                    Accounts.Id,
                	Accounts.UniqueName,
                	Accounts.DisplayName,
                	Accounts.StatusText,
                    Resources.Path AS AvatarUrl
                FROM 
                    Accounts 
                    JOIN Projects ON Projects.AccountId = Accounts.Id
                    LEFT JOIN Resources ON Resources.Id = Accounts.AvatarResourceId
                WHERE
                    Projects.Slug = @Slug;

                -- Select tags
                SELECT
                    Tags.Tag
                FROM 
                    Tags 
                    JOIN ProjectTags ON ProjectTags.TagId = Tags.Id
                    JOIN Projects On Projects.Id = ProjectTags.ProjectId
                WHERE 
                    Projects.Slug = @Slug;

                -- Select images
                SELECT
                    Resources.Path
                FROM 
                    Resources 
                    JOIN ProjectImages ON ProjectImages.Id = Resources.Id
                    JOIN Projects ON Projects.Id = ProjectImages.ProjectId
                WHERE
                    Projects.Slug = @Slug
                ORDER BY
                    ProjectImages.ImageOrder ASC;
                """,
                new { message.Slug });

            var project = await results.ReadSingleOrDefaultAsync<ProjectResponse>();
            if (project is null)
                return CommonErrors.ResourceNotFound;

            project.Account = await results.ReadSingleAsync<AccountResponse>();
            project.Account.AvatarUrl = _storage.GetUrl(project.Account.AvatarUrl!);

            project.Tags = [.. await results.ReadAsync<string>()];

            var imagePaths = (await results.ReadAsync<string>())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => _storage.GetUrl(x)!);
            project.Images = [.. imagePaths];

            return project;
        }
    }
}
