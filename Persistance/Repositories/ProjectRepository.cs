using Dapper;
using System.Data;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Entities.Tags;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Persistance.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IDbConnection _db;

        public ProjectRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Project?> GetAsync(Guid id)
        {
            Project? result = null;

            await _db.QueryAsync<Project, Resource, Tag, Project>(
                """
                SELECT
                	Projects.Id,
                	Projects.CreatedAt,
                	Projects.UpdatedAt,
                    Projects.ReleasedAt,
                	Projects.AccountId,
                	Projects.Name,
                    Projects.Slug,
                	Projects.Description,
                	Projects.IsPinned,
                    Images.Id,
                	Images.Path,
                	Images.Title,
                	Tags.Tag
                FROM
                	Projects
                	LEFT JOIN ProjectImages ON ProjectImages.ProjectId = Projects.Id
                	LEFT JOIN Images ON Images.Id = ProjectImages.ImageId
                	LEFT JOIN ProjectTags ON ProjectTags.ProjectId = Projects.Id
                	LEFT JOIN Tags ON Tags.Id = ProjectTags.TagId
                WHERE 
                    Projects.Id = @Id
                ORDER BY
                    ProjectImages.ImageOrder ASC
                """,
                (project, image, tag) =>
                {
                    result ??= project;

                    if (tag is not null)
                        result.Tags.Add(tag);
                    if (image is not null && !result.Images.Contains(image))
                        result.Images.Add(image);

                    return project;
                },
                new
                {
                    Id = id,
                },
                splitOn: "Id,Tag");

            return result;
        }

        public Task InsertAsync(Project entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Project entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Project?> GetBySlugAsync(string slug)
        {
            Project? result = null;

            await _db.QueryAsync<Project, Resource, Tag, Project>(
                """
                SELECT
                	Projects.Id,
                	Projects.CreatedAt,
                	Projects.UpdatedAt,
                    Projects.ReleasedAt,
                	Projects.AccountId,
                	Projects.Name,
                    Projects.Slug,
                	Projects.Description,
                	Projects.IsPinned,
                    Images.Id,
                	Images.Path,
                	Images.Title,
                	Tags.Tag
                FROM
                	Projects
                	LEFT JOIN ProjectImages ON ProjectImages.ProjectId = Projects.Id
                	LEFT JOIN Images ON Images.Id = ProjectImages.ImageId
                	LEFT JOIN ProjectTags ON ProjectTags.ProjectId = Projects.Id
                	LEFT JOIN Tags ON Tags.Id = ProjectTags.TagId
                WHERE 
                    Projects.Slug LIKE @Slug
                ORDER BY
                    ProjectImages.ImageOrder ASC
                """,
                (project, image, tag) =>
                {
                    result ??= project;

                    if (tag is not null)
                        result.Tags.Add(tag);
                    if (image is not null && !result.Images.Contains(image))
                        result.Images.Add(image);

                    return project;
                },
                new
                {
                    Slug = slug,
                },
                splitOn: "Id,Tag");

            return result;
        }
    }
}
