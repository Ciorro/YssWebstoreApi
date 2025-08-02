using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class UploadImageCommandHandler
        : ICommandHandler<UploadImageCommand, Result>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IProjectStorage _projectStorage;
        private readonly TimeProvider _timeProvider;

        public UploadImageCommandHandler(IRepository<Project> projectRepository, IProjectStorage projectStorage, TimeProvider timeProvider)
        {
            _projectRepository = projectRepository;
            _projectStorage = projectStorage;
            _timeProvider = timeProvider;
        }

        public async Task<Result> HandleAsync(UploadImageCommand message, CancellationToken cancellationToken = default)
        {
            var project = await _projectRepository.GetAsync(message.ProjectId);

            if (project is null)
            {
                return CommonErrors.ResourceNotFound;
            }
            if (project.AccountId != message.AccountId)
            {
                return AuthErrors.AccessDenied;
            }

            string path = await _projectStorage.UploadImage(
                message.ProjectId, message.Image);
            
            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;
            var id = Guid.CreateVersion7(creationTime);

            project.Images.Add(new Resource
            {
                Id = id,
                CreatedAt = creationTime,
                UpdatedAt = creationTime,
                Path = path,
            });

            await _projectRepository.UpdateAsync(project);
            return Result.Ok();
        }
    }
}
