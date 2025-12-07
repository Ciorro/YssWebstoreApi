using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class UploadIconCommandHandler
        : ICommandHandler<UploadIconCommand, ValueResult<string>>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IProjectStorage _projectStorage;

        public UploadIconCommandHandler(IRepository<Project> projectRepository, IProjectStorage projectStorage)
        {
            _projectRepository = projectRepository;
            _projectStorage = projectStorage;
        }

        public async Task<ValueResult<string>> HandleAsync(UploadIconCommand message, CancellationToken cancellationToken = default)
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

            Resource iconResource = await _projectStorage.UploadIcon(
                message.ProjectId, message.Icon);
            project.Icon = iconResource;

            await _projectRepository.UpdateAsync(project);
            return iconResource.PublicUrl!;
        }
    }
}
