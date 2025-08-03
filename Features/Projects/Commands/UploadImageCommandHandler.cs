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

        public UploadImageCommandHandler(IRepository<Project> projectRepository, IProjectStorage projectStorage)
        {
            _projectRepository = projectRepository;
            _projectStorage = projectStorage;
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

            Resource imageResource = await _projectStorage.UploadImage(
                message.ProjectId, message.Image);
            project.Images.Add(imageResource);

            await _projectRepository.UpdateAsync(project);
            return Result.Ok();
        }
    }
}
