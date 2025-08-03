using Dapper;
using System.Data;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Persistance.Repositories
{
    public class ProjectRepository : IRepository<Project>
    {
        private readonly IDbConnection _db;

        public ProjectRepository(IDbConnection dbConnection)
        {
            _db = dbConnection;
            _db.Open();
        }

        public async Task<Project?> GetAsync(Guid id)
        {
            using var results = await _db.QueryMultipleAsync(
                """
                -- Select project
                SELECT
                    Projects.Id,
                    Projects.CreatedAt,
                    Projects.UpdatedAt,
                    Projects.ReleasedAt,
                    Projects.AccountId,
                    Projects.Name,
                    Projects.Slug,
                    Projects.Description,
                    Projects.IsPinned
                FROM Projects WHERE Projects.Id = @Id;

                -- Select project tags
                SELECT
                    Tags.Id,
                    Tags.Tag
                FROM 
                    Tags JOIN ProjectTags ON ProjectTags.TagId = Tags.Id
                WHERE 
                    ProjectTags.ProjectId = @Id;
                
                -- Select project icon
                SELECT 
                    Resources.Id,
                    Resources.CreatedAt,
                    Resources.UpdatedAt,
                    Resources.Path,
                    Resources.PublicUrl
                FROM
                    Resources JOIN Projects ON Projects.IconResourceId = Resources.Id
                WHERE
                    Projects.Id = @Id;

                -- Select project images
                SELECT 
                    Resources.Id,
                    Resources.CreatedAt,
                    Resources.UpdatedAt,
                    Resources.Path,
                    Resources.PublicUrl
                FROM 
                    Resources JOIN ProjectImages ON ProjectImages.Id = Resources.Id
                WHERE
                    ProjectImages.ProjectId = @Id
                ORDER BY
                    ProjectImages.ImageOrder ASC;

                -- Select project packages
                SELECT
                    Resources.Id,
                    Resources.CreatedAt,
                    Resources.UpdatedAt,
                    Resources.Path,
                    Resources.PublicUrl,
                    Packages.Name,
                    Packages.Version,
                    Packages.TargetOS,
                    Packages.Size
                FROM 
                    Packages JOIN Resources ON Resources.Id = Packages.Id
                WHERE
                    Packages.ProjectId = @Id;
                """,
                new
                {
                    Id = id
                });

            var project = await results.ReadSingleOrDefaultAsync<Project>();
            if (project is null)
                return null;

            project.Tags = [.. await results.ReadAsync<TagEntity>()];
            project.Icon = await results.ReadSingleOrDefaultAsync<Resource>();
            project.Images = [.. await results.ReadAsync<Resource>()];
            project.Packages = [.. await results.ReadAsync<Package>()];

            return project;
        }

        public async Task InsertAsync(Project entity)
        {
            using var transaction = _db.BeginTransaction();

            await _db.ExecuteAsync(
                $"""
                INSERT INTO Projects (
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    ReleasedAt,
                    AccountId,
                    Name,
                    Slug,
                    Description,
                    IsPinned
                ) VALUES (
                    @{nameof(Project.Id)},
                    @{nameof(Project.CreatedAt)},
                    @{nameof(Project.UpdatedAt)},
                    @{nameof(Project.ReleasedAt)},
                    @{nameof(Project.AccountId)},
                    @{nameof(Project.Name)},
                    @{nameof(Project.Slug)},
                    @{nameof(Project.Description)},
                    @{nameof(Project.IsPinned)}
                );
                """, entity, transaction);

            await InsertTags(entity, transaction);
            await InsertIcon(entity, transaction);
            await InsertImages(entity, transaction);
            await InsertPackages(entity, transaction);

            transaction.Commit();
        }

        public async Task UpdateAsync(Project entity)
        {
            using var transaction = _db.BeginTransaction();

            await _db.ExecuteAsync(
                $"""
                UPDATE Projects
                SET Id = @{nameof(Project.Id)},
                    CreatedAt = @{nameof(Project.CreatedAt)},
                    UpdatedAt = @{nameof(Project.UpdatedAt)},
                    ReleasedAt = @{nameof(Project.ReleasedAt)},
                    AccountId = @{nameof(Project.AccountId)},
                    Name = @{nameof(Project.Name)},
                    Slug = @{nameof(Project.Slug)},
                    Description = @{nameof(Project.Description)},
                    IsPinned = @{nameof(Project.IsPinned)}
                WHERE
                    Projects.Id = @Id
                """, entity, transaction);

            await UpsertTags(entity, transaction);
            await UpsertIcon(entity, transaction);
            await UpsertImages(entity, transaction);
            await UpsertPackages(entity, transaction);

            transaction.Commit();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var transaction = _db.BeginTransaction();

            await _db.ExecuteAsync(
                """
                DELETE FROM Projects WHERE Projects.Id = @ProjectId;
                """,
                new
                {
                    ProjectId = id
                },
                transaction);

            await DeleteTags(id, transaction);
            await DeleteIcon(id, transaction);
            await DeleteImages(id, transaction);
            await DeletePackages(id, transaction);
        }

        private async Task DeleteTags(Guid entityId, IDbTransaction transaction)
        {
            await _db.ExecuteAsync(
                """
                DELETE FROM ProjectTags WHERE ProjectTags.ProjectId = @ProjectId;
                """,
                new
                {
                    ProjectId = entityId
                },
                transaction);
        }

        private async Task InsertTags(Project entity, IDbTransaction transaction)
        {
            await _db.ExecuteAsync(
                """
                -- Insert all tag relations
                INSERT INTO ProjectTags (
                    Id,
                    ProjectId,
                    TagId
                ) VALUES (
                    @Id,
                    @ProjectId,
                    @TagId
                );
                """,
                entity.Tags.Select(
                    x => new
                    {
                        Id = Guid.CreateVersion7(),
                        ProjectId = entity.Id,
                        TagId = x.Id
                    }),
                transaction);
        }

        private async Task UpsertTags(Project entity, IDbTransaction transaction)
        {
            await DeleteTags(entity.Id, transaction);
            await InsertTags(entity, transaction);
        }

        private async Task DeleteIcon(Guid entityId, IDbTransaction transaction)
        {
            await _db.ExecuteAsync(
                """
                DELETE FROM Resources USING Projects
                WHERE Projects.Id = @ProjectId
                  AND Projects.IconResourceId = Resources.Id;
                
                UPDATE Projects 
                SET 
                    IconResourceId = NULL 
                WHERE 
                    Id = @ProjectId;
                """,
                new
                {
                    ProjectId = entityId
                },
                transaction);
        }

        private async Task InsertIcon(Project entity, IDbTransaction transaction)
        {
            if (entity.Icon is null)
            {
                return;
            }

            await _db.ExecuteAsync(
                $"""
                INSERT INTO Resources (
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    Path,
                    PublicUrl
                ) VALUES (
                    @{nameof(Resource.Id)},
                    @{nameof(Resource.CreatedAt)},
                    @{nameof(Resource.UpdatedAt)},
                    @{nameof(Resource.Path)},
                    @{nameof(Resource.PublicUrl)}
                );
                
                UPDATE Projects
                SET 
                    IconResourceId = @{nameof(Resource.Id)}
                WHERE 
                    Id = @ProjectId;
                """,
                new
                {
                    entity.Icon.Id,
                    entity.Icon.CreatedAt,
                    entity.Icon.UpdatedAt,
                    entity.Icon.Path,
                    entity.Icon.PublicUrl,
                    ProjectId = entity.Id
                },
                transaction);
        }

        private async Task UpsertIcon(Project entity, IDbTransaction transaction)
        {
            await DeleteIcon(entity.Id, transaction);
            await InsertIcon(entity, transaction);
        }

        private async Task DeleteImages(Guid entityId, IDbTransaction transaction)
        {
            await _db.ExecuteAsync(
                """
                -- Delete existing project images
                DELETE FROM Resources USING ProjectImages
                WHERE ProjectImages.ProjectId = @ProjectId
                  AND ProjectImages.Id = Resources.Id;

                -- Delete existing project images relations
                DELETE FROM ProjectImages WHERE ProjectImages.ProjectId = @ProjectId;
                """,
                new
                {
                    ProjectId = entityId
                },
                transaction);
        }

        private async Task InsertImages(Project entity, IDbTransaction transaction)
        {
            await _db.ExecuteAsync(
                $"""
                -- Insert all images
                INSERT INTO Resources (
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    Path,
                    PublicUrl
                ) VALUES (
                    @{nameof(Resource.Id)},
                    @{nameof(Resource.CreatedAt)},
                    @{nameof(Resource.UpdatedAt)},
                    @{nameof(Resource.Path)},
                    @{nameof(Resource.PublicUrl)}
                );

                -- Insert all project images relations
                INSERT INTO ProjectImages (
                    Id,
                    ProjectId,
                    ImageOrder
                ) VALUES (
                    @{nameof(Resource.Id)},
                    @ProjectId,
                    @ImageOrder
                );
                """,
                entity.Images.Select(
                    (x, i) => new
                    {
                        Id = x.Id,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        Path = x.Path,
                        PublicUrl = x.PublicUrl,
                        ProjectId = entity.Id,
                        ImageOrder = i
                    }),
                transaction);
        }

        private async Task UpsertImages(Project entity, IDbTransaction transaction)
        {
            await DeleteImages(entity.Id, transaction);
            await InsertImages(entity, transaction);
        }

        private async Task DeletePackages(Guid entityId, IDbTransaction transaction)
        {
            await _db.ExecuteAsync(
                """
                -- Delete existing package resources
                DELETE FROM Resources USING Packages
                WHERE Packages.ProjectId = @ProjectId
                  AND Packages.Id = Resources.Id;

                -- Delete existing package relations
                DELETE FROM Packages WHERE Packages.ProjectId = @ProjectId;
                """,
                new
                {
                    ProjectId = entityId
                },
                transaction);
        }

        private async Task InsertPackages(Project entity, IDbTransaction transaction)
        {
            await _db.ExecuteAsync(
                $"""
                -- Insert all images
                INSERT INTO Resources (
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    Path,
                    PublicUrl
                ) VALUES (
                    @{nameof(Resource.Id)},
                    @{nameof(Resource.CreatedAt)},
                    @{nameof(Resource.UpdatedAt)},
                    @{nameof(Resource.Path)},
                    @{nameof(Resource.PublicUrl)}
                );

                -- Insert all project images relations
                INSERT INTO Packages (
                    Id,
                    ProjectId,
                    Name,
                    Version,
                    TargetOS,
                    Size
                ) VALUES (
                    @{nameof(Package.Id)},
                    @ProjectId,
                    @{nameof(Package.Name)},
                    @{nameof(Package.Version)},
                    @{nameof(Package.TargetOS)},
                    @{nameof(Package.Size)}
                );
                """,
                entity.Packages.Select(
                    x => new
                    {
                        Id = x.Id,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        Path = x.Path,
                        PublicUrl = x.PublicUrl,
                        ProjectId = entity.Id,
                        Name = x.Name,
                        Version = x.Version,
                        TargetOS = x.TargetOS,
                        Size = x.Size
                    }),
                transaction);
        }

        private async Task UpsertPackages(Project entity, IDbTransaction transaction)
        {
            await DeletePackages(entity.Id, transaction);
            await InsertPackages(entity, transaction);
        }
    }
}
