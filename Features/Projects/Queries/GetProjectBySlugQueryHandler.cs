using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Queries
{
    public class GetProjectBySlugQueryHandler
        : IQueryHandler<GetProjectBySlugQuery, Result<ProjectResponse>>
    {
        private readonly IDbConnection _db;

        public GetProjectBySlugQueryHandler(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<Result<ProjectResponse>> HandleAsync(GetProjectBySlugQuery message, CancellationToken cancellationToken = default)
        {
            ProjectResponse? result = null;

            await _db.QueryAsync<ProjectResponse, AccountResponse, string, string, ProjectResponse>(
                """
                SELECT
                	Projects.Id,
                	Projects.CreatedAt,
                	Projects.UpdatedAt,
                	Projects.ReleasedAt,
                	Projects.Name,
                	Projects.Slug,
                	Projects.Description,
                	Accounts.Id,
                	Accounts.UniqueName,
                	Accounts.DisplayName,
                	Accounts.StatusText,
                	Tags.Tag,
                	Resources.Path
                FROM
                	Projects
                	INNER JOIN Accounts ON Accounts.Id = Projects.AccountId
                	LEFT JOIN ProjectTags ON ProjectTags.ProjectId = Projects.Id
                	LEFT JOIN Tags ON Tags.Id = ProjectTags.TagId
                	LEFT JOIN ProjectImages ON ProjectImages.ProjectId = Projects.Id
                	LEFT JOIN Resources ON Resources.Id = ProjectImages.ResourceId
                WHERE
                	Projects.Slug = @Slug
                ORDER BY
                	ProjectImages.ImageOrder
                """,
                (project, account, tag, image) =>
                {
                    result ??= project;
                    result.Account = account;

                    if (!result.Tags.Contains(tag) && !string.IsNullOrEmpty(tag))
                        result.Tags.Add(tag);
                    if (!result.Images.Contains(image) && !string.IsNullOrEmpty(image))
                        result.Images.Add(image);

                    return project;
                },
                new
                {
                    message.Slug
                },
                splitOn: "id,id,tag,path");

            if (result is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            return result;
        }
    }
}
