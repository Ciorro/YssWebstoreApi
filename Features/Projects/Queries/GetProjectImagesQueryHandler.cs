using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Resources;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Queries
{
    internal class GetProjectImagesQueryHandler
        : IQueryHandler<GetProjectImagesQuery, ValueResult<IList<ResourceResponse>>>
    {
        private readonly IRepository<Project> _projectRepository;

        public GetProjectImagesQueryHandler(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ValueResult<IList<ResourceResponse>>> HandleAsync(GetProjectImagesQuery message, CancellationToken cancellationToken = default)
        {
            var project = await _projectRepository.GetAsync(message.ProjectId);
            if (project is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            return project.Images.Select(
                x => new ResourceResponse
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Url = x.PublicUrl
                })
                .ToList();
        }
    }
}
