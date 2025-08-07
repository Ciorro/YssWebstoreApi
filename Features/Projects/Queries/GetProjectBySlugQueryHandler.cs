﻿using Dapper;
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
                	Projects.Description,
                    Resources.PublicUrl AS IconUrl
                FROM 
                    Projects LEFT JOIN Resources ON Resources.Id = Projects.IconResourceId
                WHERE 
                    Projects.Slug = @Slug;

                -- Select account
                SELECT
                    Accounts.Id,
                	Accounts.UniqueName,
                	Accounts.DisplayName,
                	Accounts.StatusText,
                    Resources.PublicUrl AS AvatarUrl
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
                    Resources.PublicUrl
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

            project.Tags = [.. await results.ReadAsync<string>()];
            project.Images = [.. await results.ReadAsync<string>()];

            return project;
        }
    }
}
