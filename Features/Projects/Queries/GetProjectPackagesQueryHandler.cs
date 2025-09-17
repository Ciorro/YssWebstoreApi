using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Packages;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Queries
{
    public class GetProjectPackagesQueryHandler
        : IQueryHandler<GetProjectPackagesQuery, Result<IList<PackageResponse>>>
    {
        private readonly IRepository<Project> _projectRepository;

        public GetProjectPackagesQueryHandler(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Result<IList<PackageResponse>>> HandleAsync(GetProjectPackagesQuery message, CancellationToken cancellationToken = default)
        {
            var project = await _projectRepository.GetAsync(message.ProjectId);
            if (project is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            return project.Packages.Select(
                x => new PackageResponse
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Name = x.Name,
                    Version = x.Version,
                    TargetOS = x.TargetOS,
                    Size = x.Size
                }).ToList();
        }
    }
}
