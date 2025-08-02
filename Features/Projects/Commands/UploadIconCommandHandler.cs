using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class UploadIconCommandHandler
        : ICommandHandler<UploadIconCommand, Result>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IProjectStorage _projectStorage;
        private readonly TimeProvider _timeProvider;

        public UploadIconCommandHandler(IRepository<Project> projectRepository, IProjectStorage projectStorage, TimeProvider timeProvider)
        {
            _projectRepository = projectRepository;
            _projectStorage = projectStorage;
            _timeProvider = timeProvider;
        }

        public async Task<Result> HandleAsync(UploadIconCommand message, CancellationToken cancellationToken = default)
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

            string path = await _projectStorage.UploadIcon(
                message.ProjectId, message.Icon);

            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;
            var id = Guid.CreateVersion7(creationTime);

            project.Icon = new Resource
            {
                Id = id,
                CreatedAt = creationTime,
                UpdatedAt = creationTime,
                Path = path
            };

            await _projectRepository.UpdateAsync(project);
            return Result.Ok();
        }
    }
}
