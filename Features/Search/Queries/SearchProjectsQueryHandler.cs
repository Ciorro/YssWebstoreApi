using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Search.Queries
{
    public class SearchProjectsQueryHandler
        : IQueryHandler<SearchProjectsQuery, Result<Page<ProjectSearchResult>>>
    {
        private readonly IDbConnection _db;
        private readonly IStorage _storage;

        public SearchProjectsQueryHandler(IDbConnection dbConnection, IStorage storage)
        {
            _db = dbConnection;
            _storage = storage;
        }

        public async Task<Result<Page<ProjectSearchResult>>> HandleAsync(SearchProjectsQuery message, CancellationToken cancellationToken = default)
        {
            var allResultsIds = (await _db.QueryAsync<Guid>(message.GetCommandDefinition())).ToList();
            var limitedResultsIds = allResultsIds
                .Skip(message.PageOptions.GetOffset())
                .Take(message.PageOptions.PageSize);

            var results = await _db.QueryAsync<ProjectSearchResult, AccountResponse, ProjectSearchResult>(
                """
                WITH Ids AS (
                    SELECT
                        Id,
                        ROW_NUMBER() OVER() AS Order
                    FROM
                        UNNEST(@Ids) AS Id
                )
                SELECT
                    Projects.Id,
                    Projects.CreatedAt,
                    Projects.UpdatedAt,
                    Projects.ReleasedAt,
                    Projects.Name,
                    Projects.Slug,
                    Projects.Description,
                    Icons.Path AS IconUrl,
                    AVG(Reviews.Rate) AS Rating,
                    ARRAY_AGG(DISTINCT Tags.Tag) AS Tags,
                    Accounts.Id,
                    Accounts.UniqueName,
                    Accounts.DisplayName,
                    Accounts.StatusText,
                    Avatars.Path AS AvatarUrl
                FROM
                    Projects
                    INNER JOIN Ids ON Ids.Id = Projects.Id
                    INNER JOIN Accounts ON Accounts.Id = Projects.AccountId
                    LEFT JOIN Reviews ON Reviews.ProjectId = Projects.Id
                    LEFT JOIN ProjectTags ON ProjectTags.ProjectId = Projects.Id
                    LEFT JOIN Tags ON Tags.Id = ProjectTags.TagId
                    LEFT JOIN Resources Icons ON Icons.Id = Projects.IconResourceId
                    LEFT JOIN Resources Avatars ON Avatars.Id = Accounts.AvatarResourceId                
                GROUP BY
                    Projects.Id,
                    Accounts.Id,
                    Icons.Id,
                    Avatars.Id,
                    Ids.Order
                ORDER BY
                    Ids.Order
                """,
                (project, account) =>
                {
                    project.Account = account;
                    project.Account.AvatarUrl = _storage.GetUrl(project.Account.AvatarUrl!);
                    project.IconUrl = _storage.GetUrl(project.IconUrl!);
                    return project;
                },
                new
                {
                    Ids = limitedResultsIds
                });


            return new Page<ProjectSearchResult>(
                pageNumber: message.PageOptions.PageNumber, 
                pageSize: message.PageOptions.PageSize, 
                totalCount: allResultsIds.Count, 
                items: results.ToList());
        }
    }
}
